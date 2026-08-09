using BuildHub.DTOs;
using BuildHub.Models;
using BuildHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuildHub.Controllers
{

    [ApiController]
    [Route("quote-negotiation")]
    public class QuoteNegotiationController : ControllerBase
    {
        private QuoteNegotiationService negotiationService;

        public QuoteNegotiationController(QuoteNegotiationService _negotiationService) // dependency injection
        {
            negotiationService = _negotiationService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            List<QuoteNegotiationOutputDto> result = negotiationService.GetAllQuoteNegotiations();

            if (result != null && result.Count > 0)
            {
                return Ok(result); // 200 OK
            }

            return NoContent(); // 204 No Content
        }

        // http://localhost:5153/quote-negotiation/GetById/3
        [HttpGet("GetById/{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            QuoteNegotiationOutputDto result = negotiationService.GetQuoteNegotiationById(id);

            if (result == null)
            {
                return NotFound(); // 404 Not Found
            }

            return Ok(result); // 200 OK
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody] QuoteNegotiationInputDto quoteNegotiation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 400 Bad Request
            }

            int createdId = negotiationService.Create(quoteNegotiation);

            return Ok(new { quoteNegotiationId = createdId }); // 200 OK  ID


        }
    }
}
