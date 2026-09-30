using Backend.Data;
using Backend.Models;
using Backend.Models.Dtos;
using Backend.Models.Dtos.Backend.Models.Dtos;
using Backend.Models.Thawani;
using Backend.Options;
using Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Backend.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _http;
        private readonly ThawaniOptions _options;
        private readonly BuildHubDbContext _db; // renamed to match your actual DbContext

        public PaymentService(HttpClient http, IOptions<ThawaniOptions> options, BuildHubDbContext db)
        {
            _http = http;
            _options = options.Value;
            _db = db;

            _http.BaseAddress = new Uri(_options.ApiBaseUrl);
            _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _http.DefaultRequestHeaders.Add("thawani-api-key", _options.SecretKey);
        }

        public async Task<CheckoutSessionResponse> CreateCheckoutSessionAsync(int orderId)
        {
            var order = await _db.Orders
                .Include(o => o.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId)
                ?? throw new KeyNotFoundException($"Order {orderId} not found.");

            var request = new ThawaniCreateSessionRequest
            {
                ClientReferenceId = order.OrderId.ToString(),
                Products = new List<ThawaniProduct>
                {
                    new()
                    {
                        Name = order.Product.Name,
                        Quantity = 1,
                        UnitAmount = (int)(order.Amount * 1000) // OMR -> baisa
                    }
                },
                SuccessUrl = _options.SuccessUrl,
                CancelUrl = _options.CancelUrl,
                Metadata = new Dictionary<string, string> { ["order_id"] = order.OrderId.ToString() }
            };

            var response = await _http.PostAsJsonAsync("checkout/session", request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ThawaniResponse<ThawaniSessionData>>()
                ?? throw new InvalidOperationException("Empty response from Thawani.");

            if (!result.Success || result.Data is null)
            {
                throw new InvalidOperationException($"Thawani session creation failed: {result.Description}");
            }

            order.ThawaniSessionId = result.Data.SessionId;
            await _db.SaveChangesAsync();

            return new CheckoutSessionResponse
            {
                SessionId = result.Data.SessionId,
                PaymentUrl = $"{_options.CheckoutBaseUrl}/{result.Data.SessionId}?key={_options.PublishableKey}"
            };
        }

        public async Task<PaymentStatusResponse> ConfirmPaymentAsync(string sessionId)
        {
            // Ask Thawani directly — never trust the redirect alone.
            var response = await _http.GetAsync($"checkout/session/{sessionId}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ThawaniResponse<ThawaniSessionData>>()
                ?? throw new InvalidOperationException("Empty response from Thawani.");

            var order = await _db.Orders.FirstOrDefaultAsync(o => o.ThawaniSessionId == sessionId)
                ?? throw new KeyNotFoundException($"No order found for session {sessionId}.");

            if (result.Success && result.Data?.PaymentStatus == "paid")
            {
                order.Status = OrderStatus.Paid;
                order.PaidAt = DateTime.UtcNow;
            }
            else
            {
                order.Status = OrderStatus.Failed;
            }

            await _db.SaveChangesAsync();

            return new PaymentStatusResponse
            {
                OrderId = order.OrderId,
                Status = order.Status.ToString()
            };
        }

        public async Task ProcessWebhookAsync(string rawBody, string timestampHeader, string signatureHeader)
        {
            var expectedSignature = ComputeHmacSignature(rawBody, timestampHeader, _options.WebhookSecret);

            var expectedBytes = Encoding.UTF8.GetBytes(expectedSignature);
            var receivedBytes = Encoding.UTF8.GetBytes(signatureHeader ?? string.Empty);

            // Constant-time compare so response timing can't leak the correct signature.
            if (expectedBytes.Length != receivedBytes.Length ||
                !CryptographicOperations.FixedTimeEquals(expectedBytes, receivedBytes))
            {
                throw new UnauthorizedAccessException("Invalid webhook signature.");
            }

            var envelope = JsonSerializer.Deserialize<ThawaniWebhookEnvelope>(rawBody);
            if (envelope is null) return;

            // Only checkout.* events carry session_id directly, which is what we store on Order.
            // payment.* events reference checkout_invoice instead — skip those unless you also store invoices.
            if (envelope.EventType is not ("checkout.completed" or "checkout.cancelled"))
            {
                return;
            }

            var data = envelope.Data.Deserialize<ThawaniSessionData>();
            if (data is null || string.IsNullOrEmpty(data.SessionId)) return;

            var order = await _db.Orders.FirstOrDefaultAsync(o => o.ThawaniSessionId == data.SessionId);
            if (order is null) return; // nothing to update — ignore silently, Thawani doesn't need a body

            order.Status = data.PaymentStatus == "paid" ? OrderStatus.Paid : OrderStatus.Failed;
            if (order.Status == OrderStatus.Paid) order.PaidAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }

        private static string ComputeHmacSignature(string body, string timestamp, string webhookSecret)
        {
            var keyBytes = Encoding.ASCII.GetBytes(webhookSecret);
            var textBytes = Encoding.ASCII.GetBytes($"{body}-{timestamp}");

            using var hmac = new HMACSHA256(keyBytes);
            var hash = hmac.ComputeHash(textBytes);

            return Convert.ToHexString(hash).ToLowerInvariant();
        }
    }
}