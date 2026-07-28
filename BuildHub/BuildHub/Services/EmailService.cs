namespace BuildHub.Services
{
    public class EmailService
    {
        public EmailService() { }

        public void SendQuoteRequestAlert(int vendorId, int quoteRequestId)
        {
            Console.WriteLine("========================================");
            Console.WriteLine($"EMAIL SENT");
            Console.WriteLine($"To: Vendor #{vendorId}");
            Console.WriteLine($"Subject: You have a new quote request");
            Console.WriteLine($"Body: A customer has sent you quote request #{quoteRequestId}. Please log in to review and respond.");
            Console.WriteLine("========================================");
        }
    }
}