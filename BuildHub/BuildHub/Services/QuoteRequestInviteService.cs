using BuildHub.DTOs;
using BuildHub.Models;
using BuildHub.Repos;

namespace BuildHub.Services
{
    public class QuoteRequestInviteService
    {
        //QuoteRequestInviteRepo repo = new QuoteRequestInviteRepo();
        //apply dependency inversion concept (goal) => using dependency injection (technique/how)
        private QuoteRequestInviteRepo repo;

        public QuoteRequestInviteService(QuoteRequestInviteRepo _repo)
        {
            repo = _repo;
        }

        public List<QuoteRequestInviteOutputDto> GetAllQuoteRequestInvite()
        {
            return repo.GetAllQuoteRequestInvites()
                       .Select(invite => new QuoteRequestInviteOutputDto
                       {
                           InviteId = invite.InviteId,
                           QuoteRequestId = invite.QuoteRequestId,
                           VendorProfileId = invite.VendorProfileId,
                           InviteStatus = invite.InviteStatus
                       })
                       .ToList();
        }

        public QuoteRequestInviteOutputDto GetQuoteRequestInviteById(int id)
        {
            QuoteRequestInvite invite = repo.GetQuoteRequestInviteById(id);
            if (invite == null)
            {
                return null;
            }

            QuoteRequestInviteOutputDto output = new QuoteRequestInviteOutputDto();
            output.InviteId = invite.InviteId;
            output.QuoteRequestId = invite.QuoteRequestId;
            output.VendorProfileId = invite.VendorProfileId;
            output.InviteStatus = invite.InviteStatus;
            return output;
        }

        public int Create(QuoteRequestInviteInputDto input)
        {
            QuoteRequestInvite invite = new QuoteRequestInvite();
            invite.QuoteRequestId = input.QuoteRequestId;
            invite.VendorProfileId = input.VendorProfileId;
            invite.InviteStatus = "Sent"; // system generated default, not from user input

            repo.Add(invite);
            return invite.InviteId;
        }

        public bool UpdateStatus(int inviteId, string newStatus)
        {
            QuoteRequestInvite invite = repo.GetQuoteRequestInviteById(inviteId);
            if (invite == null)
            {
                return false;
            }

            invite.InviteStatus = newStatus;
            repo.Update(); // SaveChanges()
            return true;
        }

        public bool Delete(int inviteId)
        {
            QuoteRequestInvite invite = repo.GetQuoteRequestInviteById(inviteId);
            if (invite == null)
            {
                return false;
            }

            repo.Delete(invite);
            return true;
        }
    }
}
