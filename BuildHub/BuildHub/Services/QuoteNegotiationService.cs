using BuildHub.Models;
using BuildHub.Repos;
using Microsoft.AspNetCore.Mvc;

namespace BuildHub.Services
{
    public class QuoteNegotiationService
    {
        
        private QuoteNegotiationRepo _repo;

        public QuoteNegotiationService(QuoteNegotiationRepo repo)
        {
            repo = _repo;
        }


        public List<QuoteNegotiationOutputDTO> GetNegotiationsByQuoteId(int quoteId)
        {
            return repo.GetNegotiationsByQuoteId(quoteId)
                     .Select(n => new QuoteNegotiationOutputDTO
                     {
                         Id = n.QuoteId,
                         QuoteId = n.QuoteId,
                         SenderId = n.SenderId,
                         ProposedPrice = n.ProposedPrice,
                         ProposeddurationDays = n.ProposeddurationDays,
                         Message = n.Message,
                         CreatedAt = n.CreatedAt,

                     })
                     .ToList();


                                                      

        }





    }
}
