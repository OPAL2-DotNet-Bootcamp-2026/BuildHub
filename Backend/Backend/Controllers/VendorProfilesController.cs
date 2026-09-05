using Backend.Models.Dtos;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class VendorProfilesController : ControllerBase
    {
        private readonly IVendorProfileService _vendorProfileService;

        public VendorProfilesController(IVendorProfileService vendorProfileService)
        {
            _vendorProfileService = vendorProfileService;
        }

        /// <summary>Lists every vendor profile.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VendorProfileResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VendorProfileResponse>>> GetAll()
        {
            return Ok(await _vendorProfileService.GetAllAsync());
        }

        /// <summary>Gets one vendor profile by id.</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(VendorProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VendorProfileResponse>> GetById(int id)
        {
            var profile = await _vendorProfileService.GetByIdAsync(id);
            return profile is null ? NotFound() : Ok(profile);
        }

        /// <summary>Opens a vendor profile over an existing Vendor-role account.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(VendorProfileResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<VendorProfileResponse>> Create(CreateVendorProfileRequest request)
        {
            var created = await _vendorProfileService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.VendorProfileId }, created);
        }

        /// <summary>Updates the vendor's own business details.</summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(VendorProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VendorProfileResponse>> Update(
            int id, UpdateVendorProfileRequest request)
        {
            var updated = await _vendorProfileService.UpdateAsync(id, request);
            return updated is null ? NotFound() : Ok(updated);
        }

        /// <summary>Deletes a vendor who has no offers or reviews.</summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(int id)
        {
            return await _vendorProfileService.DeleteAsync(id) ? NoContent() : NotFound();
        }
    }
}
