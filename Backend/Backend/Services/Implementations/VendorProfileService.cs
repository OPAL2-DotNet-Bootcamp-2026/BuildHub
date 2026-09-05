using Backend.Exceptions;
using Backend.Models.Dtos;
using Backend.Models;
using Backend.Models.Entities;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.Implementations
{
    public class VendorProfileService : IVendorProfileService
    {
        private readonly IVendorProfileRepository _vendorProfileRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICurrentUser _currentUser;

        public VendorProfileService(
            IVendorProfileRepository vendorProfileRepository,
            IUserRepository userRepository,
            ICategoryRepository categoryRepository,
            ICurrentUser currentUser)
        {
            _vendorProfileRepository = vendorProfileRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<VendorProfileResponse>> GetAllAsync()
        {
            var profiles = await _vendorProfileRepository.GetAllAsync();
            return profiles.Select(ToResponse);
        }

        public async Task<VendorProfileResponse?> GetByIdAsync(int id)
        {
            var profile = await _vendorProfileRepository.GetByIdAsync(id);
            return profile is null ? null : ToResponse(profile);
        }

        public async Task<VendorProfileResponse> CreateAsync(CreateVendorProfileRequest request)
        {
            // The profile is always opened over the caller's own account.
            var userId = _currentUser.UserId;

            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new NotFoundException($"No user with id {userId}.");

            // "The business layer over a Vendor-role user."
            if (user.Role != UserRole.Vendor)
            {
                throw new BadRequestException(
                    $"User {userId} has role {user.Role}; only a Vendor can have a vendor profile.");
            }

            if (await _categoryRepository.GetByIdAsync(request.CategoryId) is null)
            {
                throw new NotFoundException($"No category with id {request.CategoryId}.");
            }

            // One profile per vendor user - UserId is unique in the database, and this
            // check turns that into a clear 409 instead of a unique-index 500.
            if (await _vendorProfileRepository.GetByUserIdAsync(userId) is not null)
            {
                throw new ConflictException($"User {userId} already has a vendor profile.");
            }

            try
            {
                var created = await _vendorProfileRepository.CreateAsync(new VendorProfile
                {
                    UserId = userId,
                    CompanyName = request.CompanyName.Trim(),
                    VendorType = request.VendorType,
                    CategoryId = request.CategoryId,
                    City = request.City.Trim(),
                    Bio = request.Bio,
                    // A vendor never arrives pre-verified, pre-rated or pre-funded.
                    IsVerified = false,
                    AverageRating = null,
                    Balance = 0m
                });

                return ToResponse(created);
            }
            catch (DbUpdateException)
            {
                // Two requests for the same user can pass the check above and race.
                throw new ConflictException($"User {userId} already has a vendor profile.");
            }
        }

        public async Task<VendorProfileResponse?> UpdateAsync(int id, UpdateVendorProfileRequest request)
        {
            var existing = await _vendorProfileRepository.GetByIdAsync(id);
            if (existing is null) return null;

            EnsureOwnedByCaller(existing);

            if (await _categoryRepository.GetByIdAsync(request.CategoryId) is null)
            {
                throw new NotFoundException($"No category with id {request.CategoryId}.");
            }

            var updated = await _vendorProfileRepository.UpdateAsync(id, new VendorProfile
            {
                CompanyName = request.CompanyName.Trim(),
                VendorType = request.VendorType,
                CategoryId = request.CategoryId,
                City = request.City.Trim(),
                Bio = request.Bio
            });

            return updated is null ? null : ToResponse(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _vendorProfileRepository.GetByIdAsync(id);
            if (existing is null) return false;

            EnsureOwnedByCaller(existing);

            try
            {
                return await _vendorProfileRepository.DeleteAsync(id);
            }
            catch (DbUpdateException)
            {
                throw new ConflictException(
                    "This vendor cannot be deleted while they still have offers or reviews.");
            }
        }

        private void EnsureOwnedByCaller(VendorProfile profile)
        {
            if (!_currentUser.IsAdmin && profile.UserId != _currentUser.UserId)
            {
                throw new ForbiddenException(
                    $"Vendor profile {profile.VendorProfileId} belongs to another account.");
            }
        }

        private static VendorProfileResponse ToResponse(VendorProfile profile) => new()
        {
            VendorProfileId = profile.VendorProfileId,
            UserId = profile.UserId,
            CompanyName = profile.CompanyName,
            VendorType = profile.VendorType,
            CategoryId = profile.CategoryId,
            City = profile.City,
            Bio = profile.Bio,
            IsVerified = profile.IsVerified,
            AverageRating = profile.AverageRating,
            Balance = profile.Balance
        };
    }
}
