using BuildHub.DTOs;
using BuildHub.Models;
using BuildHub.Repos;

namespace BuildHub.Services
{
    public class QuoteRequestInviteService
        //QuoteRequestInviteRepo repo = new QuoteRequestInviteRepo();
        //apply dependency inversion concept (goal) => using dependency injection (technique/how)
        private QuoteRequestInviteRepo repo;
   
        public QuoteRequestInviteService(QuoteRequestInviteRepo _repo)
        {
            repo = _repo;
        }
    
        public List<QuoteRequestInviteOutputDTOs> GetAllQuoteRequestInvite()
        {
            return repo.GetAllquoteRequestInvites()
                       .Select(invite => new QuoteRequestInviteOutputDTOs
                       {
                           InviteId = invite.inviteId,
                           QuoteRequestId = invite.quoteRequestId,
                           VendorProfileId = invite.vendorProfileId,
                           InviteStatus = invite.inviteStatus
                       })
                       .ToList();
        }
        public QuoteRequestInviteOutputDTOs GetQuoteRequestInviteById(int id)
            QuoteRequestInvite invite = repo.GetquoteRequestInvitesById(id);
            if (invite == null)
            {
                return null;
            }
            QuoteRequestInviteOutputDTOs output = new QuoteRequestInviteOutputDTOs();
            output.InviteId = invite.inviteId;
            output.QuoteRequestId = invite.quoteRequestId;
            output.VendorProfileId = invite.vendorProfileId;
            output.InviteStatus = invite.inviteStatus;
            return output;
}

        public int Create(QuoteRequestInviteInputDTOs input)
