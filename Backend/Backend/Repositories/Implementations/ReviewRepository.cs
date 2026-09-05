using Backend.Data;
using Backend.Models;
using Backend.Models.Entities;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories.Implementations
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly BuildHubDbContext _context;

        public ReviewRepository(BuildHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetAllAsync() =>
            await _context.Reviews.AsNoTracking().ToListAsync();

        public async Task<Review?> GetByIdAsync(int id) =>
            await _context.Reviews.AsNoTracking()
                .FirstOrDefaultAsync(r => r.ReviewId == id);

        public async Task<Review> CreateAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<Review?> UpdateAsync(int id, Review input)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review is null) return null;

            // The opinion itself. Changing Rating means VendorProfile.AverageRating
            // has to be recalculated, which is a service concern.
            review.Rating = input.Rating;
            review.Comment = input.Comment;

            // Deliberately not updated here:
            //   ReviewerId, VendorProfileId, AgreementId - the evidence chain proving
            //     this review is backed by a real completed job. Repointing any of them
            //     would let a genuine review be moved onto a different vendor.
            //   ReviewDate - a historical fact

            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<IEnumerable<Review>> GetByVendorProfileIdAsync(int vendorProfileId) =>
            await _context.Reviews.AsNoTracking()
                .Where(r => r.VendorProfileId == vendorProfileId).ToListAsync();

        public async Task<bool> DeleteAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review is null) return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
