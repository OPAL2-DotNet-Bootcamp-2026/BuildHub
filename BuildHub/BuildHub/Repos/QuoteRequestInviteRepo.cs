using BuildHub.Models;

namespace BuildHub.Repos
{
    public class QuoteRequestInviteRepo
    {
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
            return context.QuoteRequestInvites.FirstOrDefault(q => q.inviteId == id);
        }

        public List<QuoteRequestInvite> GetByQuoteRequestId(int quoteRequestId)
        {
            return context.QuoteRequestInvites
                           .Where(q => q.quoteRequestId == quoteRequestId)
                           .ToList();
        }

        public void Add(QuoteRequestInvite quoteRequestInvite)
        {
            context.QuoteRequestInvites.Add(quoteRequestInvite);
            context.SaveChanges();
        }

        public void Update()
        {
            context.SaveChanges();
        }

        public void Delete(QuoteRequestInvite quoteRequestInvite)
        {
            context.QuoteRequestInvites.Remove(quoteRequestInvite);
            context.SaveChanges();
        }
    }
}