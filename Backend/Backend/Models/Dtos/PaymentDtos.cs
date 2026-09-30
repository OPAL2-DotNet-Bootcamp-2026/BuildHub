namespace Backend.Models.Dtos
{
    namespace Backend.Models.Dtos
    {
        /// <summary>Returned after creating a checkout session — the frontend redirects here.</summary>
        public class CheckoutSessionResponse
        {
            public string SessionId { get; set; } = string.Empty;
            public string PaymentUrl { get; set; } = string.Empty;
        }

        /// <summary>Returned after confirming a session's status with Thawani.</summary>
        public class PaymentStatusResponse
        {
            public int OrderId { get; set; }
            public string Status { get; set; } = string.Empty; // "Paid" | "Pending" | "Failed"
        }
    }
}
