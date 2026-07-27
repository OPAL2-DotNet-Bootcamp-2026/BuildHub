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
            quote.quoteRequestId = quoteRequestId;
            quote.vendorProfileId = vendorProfileId;
            quote.price = price;
            quote.durationDays = durationDays;
            quote.status = "Pending";
            quote.submittedAt = DateTime.Now;

            quoteRepo.Add(quote);

            // find the customer who created this request, then notify them
            QuoteRequest request = quoteRequestRepo.GetQuoteRequestById(quoteRequestId);
            int customerUserId = request.Project.ClientId;   // <-- depends on Dev B exposing the client (adjust if reachable differently)
            notificationService.CreateNotification(customerUserId, "A vendor submitted a quote for your request", "QuoteSubmitted");

            return quote.quoteId;
        }

        // (2) customer accepts a quote -> mark accepted, notify vendor, trigger contract
        public bool AcceptQuote(int quoteId)
        {
            Quote quote = quoteRepo.GetById(quoteId);

            if (quote == null)
                return false;

            // Business rule: mark this quote as accepted
            quote.status = "Accepted";
            quoteRepo.Update();

            // notify the vendor who submitted the quote
            VendorProfileResponseDTO vendor = vendorProfileRepo.GetById(quote.vendorProfileId);
            int vendorUserId = vendor.UserId;   // <-- depends on Dev A's VendorProfile shape
            notificationService.CreateNotification(vendorUserId, "Your quote was accepted", "QuoteAccepted");

            // trigger contract generation (Dev D)
            DateTime startDate = DateTime.Now;
            DateTime finishDate = startDate.AddDays(quote.durationDays);

            contractService.CreateContractOnQuoteAcceptance(quote.quoteId, quote.price, startDate, finishDate);
            return true;
        }

        public QuoteOutputDTO GetById(int quoteId)
        {
            Quote quote = quoteRepo.GetById(quoteId);

            if (quote == null)
                return null;

            return MapToOutput(quote);
        }

        private QuoteOutputDTO MapToOutput(Quote q)
        {
            QuoteOutputDTO dto = new QuoteOutputDTO();
            dto.quoteId = q.quoteId;
            dto.quoteRequestId = q.quoteRequestId;
            dto.vendorProfileId = q.vendorProfileId;
            dto.price = q.price;
            dto.durationDays = q.durationDays;
            dto.status = q.status;
            dto.submittedAt = q.submittedAt;
            return dto;
        }
    }
}