
using BuildHub.Models;

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
            return _projectContext.VendorProfiles.ToList();
        }

        public VendorProfile? GetById(int id)
        {
            return _projectContext.VendorProfiles.FirstOrDefault(v => v.VendorProfileID == id);
        }

        public int Add(VendorProfile vendorProfile)
        {
            _projectContext.VendorProfiles.Add(vendorProfile);
            _projectContext.SaveChanges();
            return vendorProfile.VendorProfileID;
        }

        public void Update()
        {
            _projectContext.SaveChanges();
        }

        public void Delete(VendorProfile vendorProfile)
        {
            _projectContext.VendorProfiles.Remove(vendorProfile);
        }
    }
}
