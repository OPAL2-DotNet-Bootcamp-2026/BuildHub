using BuildHub.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildHub.Repos
{
    public class QuoteRequestRepo
    {
        private ProjectContext context;

        public QuoteRequestRepo(ProjectContext _context)
        {
            context = _context;
        }

        public List<QuoteRequest> GetAllQuoteRequest()
        {
            return context.QuoteRequests.ToList();
        }

        public QuoteRequest GetQuoteRequestById(int id)
        {
            return context.QuoteRequests.Include(q => q.Project).FirstOrDefault(q => q.qutoeRequestId == id);
        }

        public void Add(QuoteRequest quoteRequest)
        {
            context.QuoteRequests.Add(quoteRequest);
            context.SaveChanges();
        }

        public void Update()
        {
            context.SaveChanges();
        }

        public void Delete(QuoteRequest quoteRequest)
        {
            context.QuoteRequests.Remove(quoteRequest);
            context.SaveChanges();
        }
    }
}