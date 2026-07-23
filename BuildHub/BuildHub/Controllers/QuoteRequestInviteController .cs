using BuildHub.DTOs;
using BuildHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuildHub.Controllers
{
    [ApiController]
    [Route("QuoteRequestInvite")]
    public class QuoteRequestInviteController : ControllerBase
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
        [HttpGet("GetQuoteRequestInviteById/{id}")]
    
}
