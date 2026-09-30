using Backend.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        [Authorize]
        public class PaymentsController : ControllerBase
        {
            private readonly PaymentService _paymentService;

            public PaymentsController(PaymentService paymentService)
            {
                _paymentService = paymentService;
            }

            // POST: api/Payments/5/checkout
            [HttpPost("{agreementId:int}/checkout")]
            public async Task<IActionResult> CreateCheckout(
                int agreementId)
            {
                var result =
                    await _paymentService.CreateCheckoutAsync(
                        agreementId);

                return Ok(result);
            }

            // GET: api/Payments/3/verify
            [HttpGet("{paymentId:int}/verify")]
            public async Task<IActionResult> VerifyPayment(
                int paymentId)
            {
                var result =
                    await _paymentService.VerifyAsync(paymentId);

                return Ok(result);
            }
        }
    }

