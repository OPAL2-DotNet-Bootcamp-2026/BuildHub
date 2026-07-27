using BuildHub.DTOs;
using BuildHub.Services;
using Microsoft.AspNetCore.Mvc;


namespace BuildHub.Controllers
{
    [ApiController]
    [Route("api/quotes")]
    public class QuoteController : ControllerBase
    {
        private QuoteService quoteService;

        public QuoteController(QuoteService _quoteService) //dependency injection
        {
            quoteService = _quoteService;
        }

        // POST http://localhost:5153/api/quote-requests/3/quotes   (vendor submits)
        [HttpPost("/api/quote-requests/{quoteRequestId}/quotes")]
        public IActionResult SubmitQuote([FromRoute] int quoteRequestId, [FromBody] QuoteInputDTO input)
        {
            int quoteId = quoteService.SubmitQuote(
                quoteRequestId,
                input.vendorProfileId,
                input.price,
                input.durationDays
            );

            return Ok(new { quoteId = quoteId }); //200
        }

        // GET http://localhost:5153/api/quotes/3   (customer fetches)
        [HttpGet("{quoteId}")]
        public IActionResult GetQuoteById([FromRoute] int quoteId)
        {
            QuoteOutputDTO quote = quoteService.GetById(quoteId);

            if (quote == null)
                return NotFound(); // 404 notfound

            return Ok(quote); //200 succeeded
        }


    }
}