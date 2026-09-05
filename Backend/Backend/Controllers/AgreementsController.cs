using Backend.Models.Dtos;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AgreementsController : ControllerBase
    {
        private readonly IAgreementService _agreementService;

        public AgreementsController(IAgreementService agreementService)
        {
            _agreementService = agreementService;
        }

        /// <summary>Lists every agreement.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AgreementResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AgreementResponse>>> GetAll()
        {
            return Ok(await _agreementService.GetAllAsync());
        }

        /// <summary>Gets one agreement by id.</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(AgreementResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AgreementResponse>> GetById(int id)
        {
            var agreement = await _agreementService.GetByIdAsync(id);
            return agreement is null ? NotFound() : Ok(agreement);
        }

        /// <summary>
        /// Accepts an offer. In one transaction that offer becomes Accepted, every other
        /// offer on the job becomes NotSelected, the job becomes Hired, and the new
        /// agreement starts Active with its payment Held.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(AgreementResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AgreementResponse>> Create(CreateAgreementRequest request)
        {
            var created = await _agreementService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.AgreementId }, created);
        }

        /// <summary>
        /// Moves the escrow. Released completes the agreement and its job and credits the
        /// vendor's balance; Refunded cancels both. Either one is final.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(AgreementResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AgreementResponse>> Update(
            int id, UpdateAgreementRequest request)
        {
            var updated = await _agreementService.UpdateAsync(id, request);
            return updated is null ? NotFound() : Ok(updated);
        }

        /// <summary>Deletes an agreement and any reviews left on it.</summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            return await _agreementService.DeleteAsync(id) ? NoContent() : NotFound();
        }
    }
}
