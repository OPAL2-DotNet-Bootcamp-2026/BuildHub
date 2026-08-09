using BuildHub.Models;

namespace BuildHub.Repos
{
    public class QuoteRepo
    {
        private ProjectContext context;

        public QuoteRepo(ProjectContext context)
        {
            this.context = context;
        }

        // vendor submits
        public void Add(Quote quote)
        {
            context.Quotes.Add(quote);
            context.SaveChanges();
        }

        public Quote GetById(int quoteId)
        {
            return context.Quotes.FirstOrDefault(q => q.QuoteId == quoteId);
        }

        // status change is done by the service, then committed here
        public void Update()
        {
            context.SaveChanges();
        }
    }
}