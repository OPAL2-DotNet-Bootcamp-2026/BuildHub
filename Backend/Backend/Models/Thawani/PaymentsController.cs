using Backend.Models.Dtos;
using Backend.Models.Dtos.Backend.Models.Dtos;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("checkout/{orderId:int}")]
        [ProducesResponseType(typeof(CheckoutSessionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CheckoutSessionResponse>> CreateCheckoutSession(int orderId)
        {
            try
            {
                var session = await _paymentService.CreateCheckoutSessionAsync(orderId);
                return Ok(session);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [AllowAnonymous]
        [HttpGet("callback")]
        [ProducesResponseType(typeof(PaymentStatusResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaymentStatusResponse>> Callback([FromQuery] string session_id)
        {
            var status = await _paymentService.ConfirmPaymentAsync(session_id);
            return Ok(status);
        }

        [AllowAnonymous]
        [HttpPost("webhook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Webhook()
        {
            Request.EnableBuffering();

            using var reader = new StreamReader(Request.Body, Encoding.UTF8, leaveOpen: true);
            var rawBody = await reader.ReadToEndAsync();
            Request.Body.Position = 0;

            var timestamp = Request.Headers["thawani-timestamp"].ToString();
            var signature = Request.Headers["thawani-signature"].ToString();

            try
            {
                await _paymentService.ProcessWebhookAsync(rawBody, timestamp, signature);
                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}