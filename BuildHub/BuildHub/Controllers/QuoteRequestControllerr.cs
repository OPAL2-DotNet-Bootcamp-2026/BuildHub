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
  
    
}

