using Backend.Exceptions;
using Backend.Models.Dtos;

namespace Backend.Services.Interfaces
{
    public interface IVendorProfileService
    {
        Task<IEnumerable<VendorProfileResponse>> GetAllAsync();

        /// <summary>Null when no vendor profile has this id.</summary>
        Task<VendorProfileResponse?> GetByIdAsync(int id);

        /// <summary>
        /// Opens the business layer over a Vendor-role account.
        /// Throws <see cref="NotFoundException"/> when the user or category does not exist,
        /// <see cref="BadRequestException"/> when the account is not a Vendor, and
        /// <see cref="ConflictException"/> when it already has a profile.
        /// </summary>
        Task<VendorProfileResponse> CreateAsync(CreateVendorProfileRequest request);

        /// <summary>
        /// Null when no vendor profile has this id.
        /// Throws <see cref="NotFoundException"/> when the category does not exist.
        /// </summary>
        Task<VendorProfileResponse?> UpdateAsync(int id, UpdateVendorProfileRequest request);

        /// <summary>
        /// False when no vendor profile has this id.
        /// Throws <see cref="ConflictException"/> when they still have offers or reviews.
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}
