using BuildHub.Models;

namespace BuildHub.Repos
{
    public class QuoteRequestRepo
    {
        private ProjectContext context;

        public QuoteRequestRepo(ProjectContext _context)
        {
            context = _context;

        }

        public List<QuoteRequest> GetAllQuoteRequestRepo()
        {
            return context.QuoteRequests.ToList();

        }

        public QuoteRequest GetQuoteRequestById(int id)
        {
            return context.QuoteRequests.FirstOrDefault(Q => Q.qutoeRequestId == id);
        
        }
        public void Add(QuoteRequest quoteRequest)
        {
        context.QuoteRequests.Add(quoteRequest);
            context.SaveChanges();
        
        }
    }
}
