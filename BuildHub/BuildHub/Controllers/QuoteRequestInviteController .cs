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
    
}
