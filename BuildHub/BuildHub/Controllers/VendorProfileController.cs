using BuildHub.Models;
using BuildHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildHub.Controllers
{
    [ApiController]
    [Route("VendorProfile")]
    public class VendorProfileController : ControllerBase
    {
        private VendorProfileService _vendorProfileService;

        public VendorProfileController(VendorProfileService vendorProfileService)
        {
            _vendorProfileService = vendorProfileService;
        }

        [HttpGet("GetAllVendorProfiles")]
        public IActionResult GetAllVendorProfiles() 
        {
            List<VendorProfile> vendorProfiles = _vendorProfileService.GetAllVendorProfiles();
            if (vendorProfiles.Count <= 0)
            {
                return NoContent();
            }

            return Ok(vendorProfiles);
        }
    }
}
