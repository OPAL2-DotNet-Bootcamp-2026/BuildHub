using BuildHub.DTOs;
using BuildHub.Models;
using BuildHub.Repos;

namespace BuildHub.Services
{
    public class QuoteService
    {
        private QuoteRepo quoteRepo;
        private NotificationService notificationService;
        private ContractService contractService;      // Dev D
        private QuoteRequestRepo quoteRequestRepo;     // Dev B - read-only lookup
        private VendorProfileRepo vendorProfileRepo;   // Dev A - read-only lookup

        public QuoteService(
            QuoteRepo quoteRepo,
            NotificationService notificationService,
            ContractService contractService,
            QuoteRequestRepo quoteRequestRepo,
            VendorProfileRepo vendorProfileRepo)
        {
            this.quoteRepo = quoteRepo;
            this.notificationService = notificationService;
            this.contractService = contractService;
            this.quoteRequestRepo = quoteRequestRepo;
            this.vendorProfileRepo = vendorProfileRepo;
        }

        // (1) vendor submits a quote against a quote request -> notify the customer
        public int SubmitQuote(int quoteRequestId, int vendorProfileId, decimal price, int durationDays)
        {
            Quote quote = new Quote();
            quote.QuoteRequestId = quoteRequestId;
            quote.VendorProfileId = vendorProfileId;
            quote.Price = price;
            quote.DurationDays = durationDays;
            quote.Status = "Pending";
            quote.SubmittedAt = DateTime.Now;

            quoteRepo.Add(quote);

            // find the customer who created this request, then notify them
            QuoteRequest request = quoteRequestRepo.GetQuoteRequestById(quoteRequestId);
            //int customerUserId = request.Project.ClientId;   // <-- depends on Dev B exposing the client (adjust if reachable differently)
            //notificationService.CreateNotification(customerUserId, "A vendor submitted a quote for your request", "QuoteSubmitted");

            return quote.QuoteId;
        }

        // (2) customer accepts a quote -> mark accepted, notify vendor, trigger contract
        public bool AcceptQuote(int quoteId)
        {
            Quote quote = quoteRepo.GetById(quoteId);

            if (quote == null)
                return false;

            //idempotent check
            if (quote.Status == "Accepted")
                return true;

            // Business rule: mark this quote as accepted
            quote.Status = "Accepted";
            quoteRepo.Update();

            // notify the vendor who submitted the quote
            VendorProfileResponseDto vendor = vendorProfileRepo.GetById(quote.VendorProfileId);
            int vendorUserId = vendor.UserId;   // <-- depends on Dev A's VendorProfile shape
            notificationService.CreateNotification(vendorUserId, "Your quote was accepted", "QuoteAccepted");

            // trigger contract generation (Dev D)
            DateTime startDate = DateTime.Now;
            DateTime finishDate = startDate.AddDays(quote.DurationDays);

            contractService.CreateContractOnQuoteAcceptance(quote.QuoteId, quote.Price, startDate, finishDate);
            return true;
        }

        public QuoteOutputDto GetById(int quoteId)
        {
            Quote quote = quoteRepo.GetById(quoteId);

            if (quote == null)
                return null;

            return MapToOutput(quote);
        }

        private QuoteOutputDto MapToOutput(Quote q)
        {
            QuoteOutputDto dto = new QuoteOutputDto();
            dto.QuoteId = q.QuoteId;
            dto.QuoteRequestId = q.QuoteRequestId;
            dto.VendorProfileId = q.VendorProfileId;
            dto.Price = q.Price;
            dto.DurationDays = q.DurationDays;
            dto.Status = q.Status;
            dto.SubmittedAt = q.SubmittedAt;
            return dto;
        }
    }
}
