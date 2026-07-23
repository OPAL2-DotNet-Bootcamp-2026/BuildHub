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
    }
}
