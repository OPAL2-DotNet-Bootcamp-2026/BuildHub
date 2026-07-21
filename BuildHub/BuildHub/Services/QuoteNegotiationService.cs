using BuildHub.Models;
using BuildHub.Repos;
using Microsoft.AspNetCore.Mvc;

namespace BuildHub.Services
{
    public class QuoteNegotiationService
    {
        
        private QuoteNegotiationRepo repo;

        public QuoteNegotiationService(QuoteNegotiationRepo _repo)
        {
            repo = _repo;
        }


        public List<QuoteNegotiationOutputDTO> GetAllQuoteNegotiations()
        {
            return repo.GetAllQuoteNegotiations()
                     .Select(n => new QuoteNegotiationOutputDTO
                     {
                         Id = n.QuoteId,
                         QuoteId = n.QuoteId,
                         SenderId = n.SenderId,
                         ProposedPrice = n.ProposedPrice,
                         ProposeddurationDays = n.ProposeddurationDays,
                         
                         CreatedAt = n.CreatedAt,

                     })
                     .ToList();


                                                      

        }





    }
}
