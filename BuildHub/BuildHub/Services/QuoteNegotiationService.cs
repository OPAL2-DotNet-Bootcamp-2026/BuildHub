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

        // 1. تم تعديل نوع المرجع المرتجع ليكون مطابقاً للنوع المستعمل داخلياً
        public QuoteNegotiationOutputDTO GetQuoteNegotiationById(int id)
        {
            QuoteNegotiation q = repo.GetAllQuoteNegotiationById(id);

            // 2. حماية الكود في حال كان السجل غير موجود (null)
            if (q == null)
            {
                return null;
            }

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
            // 3. تعديل اسم الخاصية إلى quoteNegotiationId كما هو معرف في المودل مالك
            return quoteNegotiation.quoteNegotiationId;
        }
    }
}
