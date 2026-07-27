using BuildHub.Models;

namespace BuildHub.Repos
{
    public class ReviewRepo
    {
        private ProjectContext _context;

        public ReviewRepo(ProjectContext context)
        {
            _context = context;
        }

        public List<Review> GetReviewByVendorId(int vendorId)
        {
            return _context.Reviews.Where(r => r.VendorProfileId == vendorId).ToList();
        }
    }
}
