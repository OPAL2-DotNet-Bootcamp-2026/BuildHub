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
        public QuoteRequestOutputDTOs GetQuoteRequestById(int id)
        {
            QuoteRequest q = repo.GetQuoteRequestById(id);
            if (q == null)
            {
                return null;
            }

            QuoteRequestOutputDTOs output = new QuoteRequestOutputDTOs();
            output.QuoteRequestId = q.qutoeRequestId;
            output.Description = q.description;
            output.Deadline = q.deadline;
            output.VisibilityType = q.visibilityType;
            output.Status = q.status;
            return output;
        }
        public int Create(QuoteRequestInputDTOs input)
        {
            QuoteRequest q = new QuoteRequest();
            q.projectId = input.ProjectId;
            q.categoryId = input.CategoryId;
            q.description = input.Description;
            q.deadline = input.Deadline;
            q.visibilityType = input.VisibilityType;
            q.status = "Open"; 
            repo.Add(q);
            return q.qutoeRequestId;
        }
        public bool UpdateCounte(int quoteRequestId, string newCount)
            QuoteRequest q = repo.GetQuoteRequestById(quoteRequestId);
            if (q == null)
            {
                return false;
            }
            q.status = newCount;
            repo.update(); 
            return true;
    }
}
