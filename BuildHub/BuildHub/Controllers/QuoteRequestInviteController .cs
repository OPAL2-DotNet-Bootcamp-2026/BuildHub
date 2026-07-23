using BuildHub.DTOs;
using BuildHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuildHub.Controllers
{
    [ApiController]
    [Route("QuoteRequestInvite")]
    public class QuoteRequestInviteController : ControllerBase
    {
        //QuoteRequestInviteService quoteRequestInviteService = new QuoteRequestInviteService();
        //apply dependency inversion concept
        private QuoteRequestInviteService quoteRequestInviteService;
        public QuoteRequestInviteController(QuoteRequestInviteService _quoteRequestInviteService) //dependency injection
        {
            quoteRequestInviteService = _quoteRequestInviteService;
        }

        [HttpGet("GetAllQuoteRequestInvite")]
        public IActionResult GetAllQuoteRequestInvite()
        {
            // ask the service layer for all invites
            List<QuoteRequestInviteOutputDTOs> result = quoteRequestInviteService.GetAllQuoteRequestInvite();
            if (result.Count > 0)
            {
                return Ok(result);
            }
            return NoContent(); //204 no data
        }

        [HttpGet("GetQuoteRequestInviteById/{id}")]
        public IActionResult GetQuoteRequestInviteById([FromRoute] int id)
        {
            QuoteRequestInviteOutputDTOs invite = quoteRequestInviteService.GetQuoteRequestInviteById(id);
            if (invite == null)
            {
                return NotFound(); // 404 notfound
            }
            return Ok(invite); //200 succeeded
        }

        [HttpPost("AddDTO")]
        public IActionResult AddDTO([FromBody] QuoteRequestInviteInputDTOs invite)
        {
            int inviteId = quoteRequestInviteService.Create(invite);
            return Ok(new { InviteId = inviteId }); //200, InviteId=1
        }

        [HttpPut("UpdateStatus/{inviteId}")]
        public IActionResult UpdateStatus([FromRoute] int inviteId, [FromQuery] string newStatus)
        {
            bool updated = quoteRequestInviteService.UpdateStatus(inviteId, newStatus);
            if (!updated)
                return NotFound();
            return Ok("Updated successfully");
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            bool deleted = quoteRequestInviteService.Delete(id);
            if (!deleted)
                return NotFound();
            return Ok("deleted successfully");
        }
    }
}
