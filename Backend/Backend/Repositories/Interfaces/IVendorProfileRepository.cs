using Backend.Models;
using Backend.Models.Entities;

namespace Backend.Repositories.Interfaces
{
    public interface IVendorProfileRepository
    {
        Task<IEnumerable<VendorProfile>> GetAllAsync();

        /// <summary>Returns null when no vendor profile has this id.</summary>
        Task<VendorProfile?> GetByIdAsync(int id);

        Task<VendorProfile> CreateAsync(VendorProfile vendorProfile);

        /// <summary>
        /// Updates the business details a vendor maintains themselves. Verification,
        /// rating and balance are not editable here.
        /// Returns null when the id does not exist.
        /// </summary>
        Task<VendorProfile?> UpdateAsync(int id, VendorProfile input);

        /// <summary>
        /// Adds to the vendor's earnings wallet when an agreement's escrow is released.
        /// The only way Balance ever moves, and deliberately not part of
        /// <see cref="UpdateAsync"/>. False when the id does not exist.
        /// </summary>
        Task<bool> CreditBalanceAsync(int id, decimal amount);

        /// <summary>
        /// Stores the rating recomputed from this vendor's reviews. Null when they have
        /// none left. False when the id does not exist.
        /// </summary>
        Task<bool> SetAverageRatingAsync(int id, decimal? averageRating);

        /// <summary>False when the id does not exist.</summary>
        Task<bool> DeleteAsync(int id);
    }
}
