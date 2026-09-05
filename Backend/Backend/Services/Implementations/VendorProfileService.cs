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

        public VendorProfileService(
            IVendorProfileRepository vendorProfileRepository,
            IUserRepository userRepository,
            ICategoryRepository categoryRepository)
        {
            _vendorProfileRepository = vendorProfileRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
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
            var user = await _userRepository.GetByIdAsync(request.UserId)
                ?? throw new NotFoundException($"No user with id {request.UserId}.");

            // "The business layer over a Vendor-role user."
            if (user.Role != UserRole.Vendor)
            {
                throw new BadRequestException(
                    $"User {request.UserId} has role {user.Role}; only a Vendor can have a vendor profile.");
            }

            if (await _categoryRepository.GetByIdAsync(request.CategoryId) is null)
            {
                throw new NotFoundException($"No category with id {request.CategoryId}.");
            }

            // One profile per vendor user - UserId is unique in the database, and this
            // check turns that into a clear 409 instead of a unique-index 500.
            var existing = await _vendorProfileRepository.GetAllAsync();
            if (existing.Any(v => v.UserId == request.UserId))
            {
                throw new ConflictException($"User {request.UserId} already has a vendor profile.");
            }

            try
            {
                var created = await _vendorProfileRepository.CreateAsync(new VendorProfile
                {
                    UserId = request.UserId,
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
                throw new ConflictException($"User {request.UserId} already has a vendor profile.");
            }
        }

        public async Task<VendorProfileResponse?> UpdateAsync(int id, UpdateVendorProfileRequest request)
        {
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
