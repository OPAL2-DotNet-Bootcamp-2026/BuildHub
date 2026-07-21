using BuildHub.DTOs;
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
                         quoteNegotiationId = n.quoteNegotiationId,
                         userId = n.userId,
                         proposedPrice = n.proposedPrice,
                         proposedDurationDays = n.proposedDurationDays,
                         createIn = n.createIn,

                     })
                     .ToList();
                                                 
        }

        public QuoteNegotiationAllOutputDTO GetQuoteNegotiationById(int id)
        {

            QuoteNegotiation q = repo.GetAllQuoteNegotiationById(id);

            QuoteNegotiationOutputDTO output = new QuoteNegotiationOutputDTO();
            output.quoteNegotiationId = q.quoteNegotiationId;
            output.userId = q.userId;
            output.proposedPrice = q.proposedPrice;
            output.proposedDurationDays = q.proposedDurationDays;
            output.createIn = q.createIn;
            return output;

        }


        public int Create(QuoteNegotiation quoteNegotiation)
        {
            repo.Add(quoteNegotiation);
            return quoteNegotiation.id;

        }










    }
}
