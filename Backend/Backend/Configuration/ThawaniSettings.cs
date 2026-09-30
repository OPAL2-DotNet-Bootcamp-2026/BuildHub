namespace Backend.Configuration
{
    public class ThawaniSettings          //for Configuration → no database table
    {
        // The section name inside appsettings
        public const string SectionName = "Thawani";

        // Thawani testing API
        public string ApiBaseUrl { get; set; }
            = "https://uatcheckout.thawani.om";

        // Thawani testing payment page
        public string CheckoutBaseUrl { get; set; }
            = "https://uatcheckout.thawani.om";

        // Private key — backend only
        public string SecretKey { get; set; } = string.Empty;

        // Used when creating the checkout URL
        public string PublishableKey { get; set; } = string.Empty;

        // Where Thawani redirects after successful payment
        public string SuccessUrl { get; set; } = string.Empty;

        // Where Thawani redirects after cancellation
        public string CancelUrl { get; set; } = string.Empty;

        // Checks that all required settings exist
        public bool IsConfigured =>
            !string.IsNullOrWhiteSpace(SecretKey)
            && !string.IsNullOrWhiteSpace(PublishableKey)
            && !string.IsNullOrWhiteSpace(SuccessUrl)
            && !string.IsNullOrWhiteSpace(CancelUrl);
    }

}
