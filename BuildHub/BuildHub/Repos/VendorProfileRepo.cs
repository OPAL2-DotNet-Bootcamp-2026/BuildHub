using BuildHub.DTOs;
using BuildHub.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildHub.Repos
{

    public class VendorProfileRepo
    {
        private ProjectContext _projectContext;

        public VendorProfileRepo(ProjectContext projectContext)
        {
            _projectContext = projectContext;
        }

        public List<VendorProfile> GetAll()
        {
            return _projectContext.VendorProfiles.Include(v => v.Reviews).ThenInclude(r => r.Reviewer).ToList();
        }

        public VendorProfileResponseDto? GetById(int id)
        {
            VendorProfile vendorProfile = _projectContext.VendorProfiles.Include(v => v.Reviews).ThenInclude(r => r.Reviewer).FirstOrDefault(v => v.VendorProfileId == id);

            VendorProfileResponseDto response = new VendorProfileResponseDto()
            {
                VendorProfileId = vendorProfile.VendorProfileId,
                UserId = vendorProfile.UserId,
                CompanyName = vendorProfile.CompanyName,
                VendorType = vendorProfile.VendorType,
                City = vendorProfile.City,
                IsVerified = vendorProfile.IsVerified,
                AverageRating = vendorProfile.AverageRating,
                Balance = vendorProfile.Balance,
            };

            return response;
        }

        public int Add(VendorProfile vendorProfile)
        {
            _projectContext.VendorProfiles.Add(vendorProfile);
            _projectContext.SaveChanges();
            return vendorProfile.VendorProfileId;
        }

        public void Update()
        {
            _projectContext.SaveChanges();
        }

        public void Delete(VendorProfile vendorProfile)
        {
            _projectContext.VendorProfiles.Remove(vendorProfile);
            _projectContext.SaveChanges();
        }
    }
}
