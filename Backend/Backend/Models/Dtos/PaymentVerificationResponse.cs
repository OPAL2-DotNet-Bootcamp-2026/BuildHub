namespace Backend.Models.Dtos
{
    // Backend returns the result after checking Thawani
    public class PaymentVerificationResponse
    {

        public GatewayPaymentStatus Status { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
