using BuildHub.DTOs;
using BuildHub.Models;
using BuildHub.Repos;

namespace BuildHub.Services
{
    public class QuoteRequestService
    {
        //QuoteRequestRepo repo = new QuoteRequestRepo();
        //apply dependency inversion
        private QuoteRequestRepo repo;
        private QuoteRequestInviteRepo inviteRepo;
        private VendorProfileRepo vendorProfileRepo;
        private NotificationService notificationService;
        private EmailService emailService;

        public QuoteRequestService(
            QuoteRequestRepo _repo,
            QuoteRequestInviteRepo _inviteRepo,
            VendorProfileRepo _vendorProfileRepo,
            NotificationService _notificationService,
            EmailService _emailService)
        {
            repo = _repo;
            inviteRepo = _inviteRepo;
            vendorProfileRepo = _vendorProfileRepo;
            notificationService = _notificationService;
            emailService = _emailService;
        }

        public List<QuoteRequestOutputDto> GetAllQuoteRequest()
        {
            return repo.GetAllQuoteRequest()
                       .Select(quoteRequest => new QuoteRequestOutputDto
                       {
                           QuoteRequestId = quoteRequest.QuoteRequestId,
                           Description = quoteRequest.Description,
                           Deadline = quoteRequest.Deadline,
                           VisibilityType = quoteRequest.VisibilityType,
                           Status = quoteRequest.Status
                       })
                       .ToList();
        }

        public QuoteRequestOutputDto GetQuoteRequestById(int id)
        {
            QuoteRequest q = repo.GetQuoteRequestById(id);
            if (q == null)
            {
                return null;
            }

            QuoteRequestOutputDto output = new QuoteRequestOutputDto();
            output.QuoteRequestId = q.QuoteRequestId;
            output.Description = q.Description;
            output.Deadline = q.Deadline;
            output.VisibilityType = q.VisibilityType;
            output.Status = q.Status;
            return output;
        }


        public int Create(QuoteRequestInputDto input)
        {
            QuoteRequest q = new QuoteRequest();
            q.ProjectId = input.ProjectId;
            q.CategoryId = input.CategoryId;
            q.Description = input.Description;
            q.Deadline = input.Deadline;
            q.VisibilityType = "Direct";
            q.Status = "Open";

            repo.Add(q);

            QuoteRequestInvite invite = new QuoteRequestInvite();
            invite.QuoteRequestId = q.QuoteRequestId;
            invite.VendorProfileId = input.VendorProfileId;
            invite.InviteStatus = "Sent";
            inviteRepo.Add(invite);

            VendorProfileResponseDto vendor = vendorProfileRepo.GetById(invite.VendorProfileId);

            notificationService.CreateNotification(vendor.UserId, "New quote request received", "QuoteRequest");
            emailService.SendQuoteRequestAlert(invite.VendorProfileId, q.QuoteRequestId);

            return q.QuoteRequestId;
        }

        public bool UpdateCounte(int quoteRequestId, string newCount)
        {
            QuoteRequest q = repo.GetQuoteRequestById(quoteRequestId);
            if (q == null)
            {
                return false;
            }

            q.Status = newCount;
            repo.Update();
            return true;
        }

        public bool Delete(int quoteRequestId)
        {
            QuoteRequest q = repo.GetQuoteRequestById(quoteRequestId);
            if (q == null)
            {
                return false;
            }

            repo.Delete(q);
            return true;
        }
    }
}
