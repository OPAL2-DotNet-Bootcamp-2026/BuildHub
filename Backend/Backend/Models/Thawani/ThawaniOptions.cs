namespace Backend.Options
{
    /// <summary>Bound from the "Thawani" section of appsettings.json / user-secrets.</summary>
    public class ThawaniOptions
    {
        public const string SectionName = "Thawani";

        // Server-side only. NEVER send this to the frontend.
        public string SecretKey { get; set; } = string.Empty;

        // Safe to expose to the frontend if ever needed.
        public string PublishableKey { get; set; } = string.Empty;

        // e.g. https://uatcheckout.thawani.om/api/v1  (UAT/sandbox)
        //      https://checkout.thawani.om/api/v1     (production)
        public string ApiBaseUrl { get; set; } = string.Empty;

        // e.g. https://uatcheckout.thawani.om/pay  (UAT/sandbox)
        //      https://checkout.thawani.om/pay      (production)
        public string CheckoutBaseUrl { get; set; } = string.Empty;

        public string SuccessUrl { get; set; } = string.Empty;
        public string CancelUrl { get; set; } = string.Empty;

        // Set separately in the Thawani Merchant Portal (Webhook URL section) — NOT the same as SecretKey.
        public string WebhookSecret { get; set; } = string.Empty;
    }
}