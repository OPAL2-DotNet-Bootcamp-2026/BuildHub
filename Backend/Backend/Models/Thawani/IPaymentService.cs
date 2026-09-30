using Backend.Models.Dtos;
using Backend.Models.Dtos.Backend.Models.Dtos;

namespace Backend.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<CheckoutSessionResponse> CreateCheckoutSessionAsync(int orderId);
        Task<PaymentStatusResponse> ConfirmPaymentAsync(string sessionId);
        Task ProcessWebhookAsync(string rawBody, string timestampHeader, string signatureHeader);
    }
}