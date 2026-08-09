using BuildHub.DTOs;
using BuildHub.Models;
using BuildHub.Repos;

namespace BuildHub.Services
{
    public class QuoteNegotiationService
    {
        private QuoteNegotiationRepo repo;

        public QuoteNegotiationService(QuoteNegotiationRepo _repo)
        {
            repo = _repo;
        }

        public List<QuoteNegotiationOutputDto> GetAllQuoteNegotiations()
        {
            return repo.GetAllQuoteNegotiations()
                       .Select(n => new QuoteNegotiationOutputDto
                       {
                           QuoteNegotiationId = n.QuoteNegotiationId,
                           UserId = n.UserId,
                           ProposedPrice = n.ProposedPrice,
                           ProposedDurationDays = n.ProposedDurationDays,
                           CreatedAt = n.CreatedAt,
                       })
                       .ToList();
        }

        public QuoteNegotiationOutputDto GetQuoteNegotiationById(int id)
        {
            QuoteNegotiation q = repo.GetQuoteNegotiationById(id);

            if (q == null)
            {
                return null;
            }

            QuoteNegotiationOutputDto output = new QuoteNegotiationOutputDto();
            output.QuoteNegotiationId = q.QuoteNegotiationId;
            output.UserId = q.UserId;
            output.ProposedPrice = q.ProposedPrice;
            output.ProposedDurationDays = q.ProposedDurationDays;
            output.CreatedAt = q.CreatedAt;

            return output;
        }

        public int Create(QuoteNegotiationInputDto input)
        {
            QuoteNegotiation quoteNegotiation = new QuoteNegotiation();
            quoteNegotiation.UserId = input.UserId;
            quoteNegotiation.QuoteId = input.QuoteId;
            quoteNegotiation.ProposedPrice = input.ProposedPrice;
            quoteNegotiation.ProposedDurationDays = input.ProposedDurationDays;
            quoteNegotiation.Message = input.Message;
            quoteNegotiation.CreatedAt = DateTime.Now;

            repo.Add(quoteNegotiation);

            return quoteNegotiation.QuoteNegotiationId;
        }
    }
}
