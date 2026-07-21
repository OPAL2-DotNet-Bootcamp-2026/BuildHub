using BuildHub.DTOs;
using BuildHub.Models;
using BuildHub.Repos;

namespace BuildHub.Services
{
    public class QuoteRequestService
    {
        //QuoteRequestRepo repo = new QuoteRequestRepo();
        //apply dependency inversion 
        private QuoteRequestRepo repo;
        
        
        public List<QuoteRequestOutputDTOs> GetAllQuoteRequest()
        {
            return repo.GetAllQuoteRequest()
                       .Select(quoteRequest => new QuoteRequestOutputDTOs
                       {
                           QuoteRequestId = quoteRequest.qutoeRequestId,
                           Description = quoteRequest.description,
                           Deadline = quoteRequest.deadline,
                           VisibilityType = quoteRequest.visibilityType,
                           Status = quoteRequest.status
                       })
                       .ToList();
        }
    }
}
