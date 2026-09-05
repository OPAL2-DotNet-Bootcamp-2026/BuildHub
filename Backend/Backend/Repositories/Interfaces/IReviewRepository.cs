using Backend.Models;
using Backend.Models.Entities;

namespace Backend.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllAsync();

        /// <summary>Returns null when no review has this id.</summary>
        Task<Review?> GetByIdAsync(int id);

        Task<Review> CreateAsync(Review review);

        /// <summary>
        /// Updates the opinion only - the rating and its comment. The evidence chain
        /// behind it is fixed. Returns null when the id does not exist.
        /// </summary>
        Task<Review?> UpdateAsync(int id, Review input);

        /// <summary>Every review left on one vendor - used to recompute their average.</summary>
        Task<IEnumerable<Review>> GetByVendorProfileIdAsync(int vendorProfileId);

        /// <summary>False when the id does not exist.</summary>
        Task<bool> DeleteAsync(int id);
    }
}
