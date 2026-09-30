using Backend.Configuration;
using Backend.Exceptions;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Backend.Services.Implementations
{
    public class ThawaniGateway
    {
            private readonly HttpClient _httpClient;
            private readonly ThawaniSettings _settings;

            public ThawaniGateway(
                HttpClient httpClient,
                IOptions<ThawaniSettings> options)
            {
                _httpClient = httpClient;
                _settings = options.Value;
            }

            // Create a Thawani checkout session
            public async Task<ThawaniCheckoutResult> CreateCheckoutAsync(
                string referenceId,
                string productName,
                long amountInBaisa,
                Dictionary<string, string> metadata)
            {
                CheckConfiguration();

                // JSON sent to Thawani
                var requestBody = new
                {
                    client_reference_id = referenceId,

                    mode = "payment",

                    products = new[]
                    {
                    new
                    {
                        name = productName,
                        quantity = 1,
                        unit_amount = amountInBaisa
                    }
                },

                    success_url = _settings.SuccessUrl,

                    cancel_url = _settings.CancelUrl,

                    metadata
                };

                // Prepare the HTTP request
                using var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    $"{_settings.ApiBaseUrl.TrimEnd('/')}/api/v1/checkout/session");

                request.Headers.Add(
                    "thawani-api-key",
                    _settings.SecretKey);

                request.Content = JsonContent.Create(requestBody);

                // Send request to Thawani
                using var response =
                    await _httpClient.SendAsync(request);

                var responseBody =
                    await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new BadRequestException(
                        $"Thawani rejected the request. Status: {(int)response.StatusCode}");
                }

                using var json = JsonDocument.Parse(responseBody);

                // Get the data object from Thawani JSON
                if (!json.RootElement.TryGetProperty(
                        "data",
                        out var data))
                {
                    throw new BadRequestException(
                        "Thawani did not return checkout data.");
                }

                // Get session_id
                if (!data.TryGetProperty(
                        "session_id",
                        out var sessionProperty))
                {
                    throw new BadRequestException(
                        "Thawani did not return a session ID.");
                }

                var sessionId = sessionProperty.GetString();

                if (string.IsNullOrWhiteSpace(sessionId))
                {
                    throw new BadRequestException(
                        "The Thawani session ID is empty.");
                }

                // Create the hosted checkout URL
                var checkoutUrl =
                    $"{_settings.CheckoutBaseUrl.TrimEnd('/')}/pay/"
                    + $"{Uri.EscapeDataString(sessionId)}"
                    + $"?key={Uri.EscapeDataString(_settings.PublishableKey)}";

                return new ThawaniCheckoutResult(
                    sessionId,
                    checkoutUrl);
            }

            // Check the payment after returning from Thawani
            public async Task<ThawaniSessionResult> GetSessionAsync(
                string sessionId)
            {
                CheckConfiguration();

                using var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    $"{_settings.ApiBaseUrl.TrimEnd('/')}"
                    + "/api/v1/checkout/session/"
                    + Uri.EscapeDataString(sessionId));

                request.Headers.Add(
                    "thawani-api-key",
                    _settings.SecretKey);

                using var response =
                    await _httpClient.SendAsync(request);

                var responseBody =
                    await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new BadRequestException(
                        $"Could not verify the Thawani session. Status: {(int)response.StatusCode}");
                }

                using var json = JsonDocument.Parse(responseBody);

                if (!json.RootElement.TryGetProperty(
                        "data",
                        out var data))
                {
                    throw new BadRequestException(
                        "Thawani did not return session data.");
                }

                var paymentStatus =
                    data.TryGetProperty(
                        "payment_status",
                        out var statusProperty)
                        ? statusProperty.GetString()
                        : "unpaid";

                var paymentId =
                    data.TryGetProperty(
                        "payment_id",
                        out var paymentProperty)
                        ? paymentProperty.GetString()
                        : null;

                return new ThawaniSessionResult(
                    paymentStatus ?? "unpaid",
                    paymentId);
            }

            // Make sure the keys and URLs exist
            private void CheckConfiguration()
            {
                if (!_settings.IsConfigured)
                {
                    throw new BadRequestException(
                        "Thawani is not configured. Add the UAT keys using user-secrets.");
                }
            }
        }

        // Result after creating checkout
        public record ThawaniCheckoutResult(
            string SessionId,
            string CheckoutUrl);

        // Result after checking the session
        public record ThawaniSessionResult(
            string PaymentStatus,
            string? PaymentId);
    }


