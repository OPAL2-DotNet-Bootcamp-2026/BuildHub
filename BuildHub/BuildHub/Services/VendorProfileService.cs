using BuildHub.Models;
using BuildHub.Repos;

namespace BuildHub.Services
{
    public class VendorProfileService
    {
        private VendorProfileRepo _vendorProfileRepo;

        public VendorProfileService(VendorProfileRepo vendorProfileRepo)
        {
            _vendorProfileRepo = vendorProfileRepo;
        }

        public List<VendorProfile> GetAllVendorProfiles() 
        { 
            return _vendorProfileRepo.GetAll();
        }
    }
}
