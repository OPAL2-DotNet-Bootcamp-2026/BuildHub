using BuildHub.Models;

namespace BuildHub.Repos
{
    public class QuoteRequestInviteRepo
        private ProjectContext context;

        public QuoteRequestInviteRepo(ProjectContext _context)
        {
            context = _context;

        }
        public List<QuoteRequestInvite> GetAllquoteRequestInvites()
        {

            return context.QuoteRequestInvites.ToList();


        }
        public QuoteRequestInvite GetquoteRequestInvitesById(int id)
        {
            return context.QuoteRequestInvites.FirstOrDefault(Q => Q.inviteId == id);

        }
        public void Add(QuoteRequestInvite quoteRequestInvite)
        {
            context.QuoteRequestInvites.Add(quoteRequestInvite);
            context.SaveChanges();

        }
}
