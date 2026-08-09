using BuildHub.DTOs;
using BuildHub.Models;
using BuildHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BuildHub.Controllers
{
    [ApiController]
    [Route("api/quote-requests")] 
    public class QuoteRequestController : ControllerBase
    {
        private QuoteRequestService quoteRequestService;

        public QuoteRequestController(QuoteRequestService _quoteRequestService)
        {
            quoteRequestService = _quoteRequestService;
        }


        [HttpGet("GetAllQuoteRequest")]
        public IActionResult GetAllQuoteRequest()
        {
            List<QuoteRequestOutputDto> result = quoteRequestService.GetAllQuoteRequest();

            if (result.Count > 0)
            {
                return Ok(result); // 200 - return the list
            }
            return NoContent(); // 204 - no quote requests exist yet
        }

        [HttpGet("{id}")]
        public IActionResult GetQuoteRequestById([FromRoute] int id)
        {
            QuoteRequestOutputDto quoteRequest = quoteRequestService.GetQuoteRequestById(id);

            if (quoteRequest == null)
            {
                return NotFound(); // 404 - no quote request with this id
            }
            return Ok(quoteRequest); // 200 - found, return it
        }


        [HttpPost]
        public IActionResult AddDTO([FromBody] QuoteRequestInputDto quoteRequest)
        {

            int quoteRequestId = quoteRequestService.Create(quoteRequest);

            return Ok(new { QuoteRequestId = quoteRequestId }); // 200 - return the new id
        }


        [HttpPut("UpdateCounte/{quoteRequestId}")]
        public IActionResult UpdateCounte([FromRoute] int quoteRequestId, [FromQuery] string newStatus)
        {
            bool updated = quoteRequestService.UpdateCounte(quoteRequestId, newStatus);

            if (!updated)
                return NotFound(); // 404 - no quote request with this id
            return Ok("Updated successfully"); // 200 - status changed
        }


        [HttpDelete("Delete/{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            bool deleted = quoteRequestService.Delete(id);

            if (!deleted)
                return NotFound(); // 404 - nothing to delete
            return Ok("deleted successfully"); // 200 - deletion succeeded
        }
    }
}