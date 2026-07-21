using BuildHub.DTOs;
using BuildHub.Models;
using BuildHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BuildHub.Controllers
{
    [ApiController]
    [Route("QuoteRequest")]
    public class QuoteRequestController : ControllerBase
        //QuoteRequestService quoteRequestService = new QuoteRequestService();
        //apply dependency inversion concept
        private QuoteRequestService quoteRequestService;
        public QuoteRequestController(QuoteRequestService _quoteRequestService) //dependency injection
        {
            quoteRequestService = _quoteRequestService;
        }
  
    
        [HttpGet("GetAllQuoteRequest")]
        public IActionResult GetAllQuoteRequest()
            // ask the service layer for all quote requests
            List<QuoteRequestOutputDTOs> result = quoteRequestService.GetAllQuoteRequest();
            if (result.Count > 0)
            {
                return Ok(result);
            }
            return NoContent(); //204 no data
}

        [HttpGet("GetQuoteRequestById/{id}")]
        public IActionResult GetQuoteRequestById([FromRoute] int id)
            QuoteRequestOutputDTOs quoteRequest = quoteRequestService.GetQuoteRequestById(id);
            if (quoteRequest == null)
            {
                return NotFound(); // 404 notfound
            }
            return Ok(quoteRequest); //200 succeeded
        [HttpPost("AddDTO")]
        public IActionResult AddDTO([FromBody] QuoteRequestInputDTOs quoteRequest)
            int quoteRequestId = quoteRequestService.Create(quoteRequest);
            return Ok(new { QuoteRequestId = quoteRequestId }); //200, QuoteRequestId=1

        [HttpPut("UpdateCounte/{quoteRequestId}")]
        public IActionResult UpdateCounte([FromRoute] int quoteRequestId, [FromQuery] string newStatus)
            bool updated = quoteRequestService.UpdateCounte  (quoteRequestId, newStatus);
            if (!updated)
                return NotFound();
            return Ok("Updated successfully");
        [HttpDelete("Delete/{id}")]
