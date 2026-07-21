using BuildHub.Models;

namespace BuildHub.Repos
{
    public class QuoteNegotiationRepo
    {
        private ProjectContext context;

        public QuoteNegotiationRepo(ProjectContext _context)
        {
            context = _context;

        }

        public List<QuoteNegotiation> GetAllQuoteNegotiations()
        {
            return context.QuoteNegotiations.ToList();

        }



        public QuoteNegotiation GetAllQuoteNegotiationById(int id)

        {
            return context.QuoteNegotiations.FirstOrDefault(q => q.quoteNegotiationId == id);
        
        }

        public void  Add (QuoteNegotiation QuoteNegotiationq)

        {
            context.QuoteNegotiations.Add(QuoteNegotiationq);
            context.SaveChanges();

        }

        public void Update()
        {
            context.SaveChanges ();
        }

        public void Delete(QuoteNegotiation quoteNegotiation)
        {
            context.QuoteNegotiations.Remove(quoteNegotiation);
            context.SaveChanges();
        
        }





    }
}
