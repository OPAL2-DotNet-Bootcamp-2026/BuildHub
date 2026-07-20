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

    }
}
