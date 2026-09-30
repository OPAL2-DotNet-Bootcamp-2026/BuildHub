namespace Backend.Models.Dtos
{
    // Backend returns the Thawani payment link
    public class PaymentCheckoutResponse
    {
        public int PaymentId { get; set; }

        public string CheckoutUrl { get; set; } = string.Empty;


    }
}
