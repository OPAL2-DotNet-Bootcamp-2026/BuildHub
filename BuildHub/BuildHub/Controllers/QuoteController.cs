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


    }
}