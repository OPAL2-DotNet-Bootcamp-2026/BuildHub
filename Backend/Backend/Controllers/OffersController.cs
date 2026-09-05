using Backend.Models;
using Backend.Models.Dtos;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class OffersController : ControllerBase
    {
        private readonly IOfferService _offerService;

        public OffersController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        /// <summary>Lists every offer.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OfferResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OfferResponse>>> GetAll()
        {
            return Ok(await _offerService.GetAllAsync());
        }

        /// <summary>Gets one offer by id.</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(OfferResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OfferResponse>> GetById(int id)
        {
            var offer = await _offerService.GetByIdAsync(id);
            return offer is null ? NotFound() : Ok(offer);
        }

        /// <summary>
        /// Submits an offer on an Open job. One offer per vendor per job; it starts Pending.
        /// </summary>
        [Authorize(Roles = nameof(UserRole.Vendor))]
        [HttpPost]
        [ProducesResponseType(typeof(OfferResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<OfferResponse>> Create(CreateOfferRequest request)
        {
            var created = await _offerService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.OfferId }, created);
        }

        /// <summary>Corrects a quote. Allowed only while the offer is still Pending.</summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(OfferResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<OfferResponse>> Update(int id, UpdateOfferRequest request)
        {
            var updated = await _offerService.UpdateAsync(id, request);
            return updated is null ? NotFound() : Ok(updated);
        }

        /// <summary>Deletes an offer that has not been accepted into an agreement.</summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(int id)
        {
            return await _offerService.DeleteAsync(id) ? NoContent() : NotFound();
        }
    }
}
