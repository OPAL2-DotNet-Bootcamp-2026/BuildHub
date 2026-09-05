using Backend.Data;
using Backend.Models;
using Backend.Models.Entities;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories.Implementations
{
    public class VendorProfileRepository : IVendorProfileRepository
    {
        private readonly BuildHubDbContext _context;

        public VendorProfileRepository(BuildHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VendorProfile>> GetAllAsync() =>
            await _context.VendorProfiles.AsNoTracking().ToListAsync();

        public async Task<VendorProfile?> GetByIdAsync(int id) =>
            await _context.VendorProfiles.AsNoTracking()
                .FirstOrDefaultAsync(v => v.VendorProfileId == id);

        public async Task<VendorProfile> CreateAsync(VendorProfile vendorProfile)
        {
            _context.VendorProfiles.Add(vendorProfile);
            await _context.SaveChangesAsync();
            return vendorProfile;
        }

        public async Task<VendorProfile?> UpdateAsync(int id, VendorProfile input)
        {
            var vendorProfile = await _context.VendorProfiles.FindAsync(id);
            if (vendorProfile is null) return null;

            // Business details the vendor maintains themselves.
            vendorProfile.CompanyName = input.CompanyName;
            vendorProfile.VendorType = input.VendorType;
            vendorProfile.CategoryId = input.CategoryId;
            vendorProfile.City = input.City;
            vendorProfile.Bio = input.Bio;

            // Deliberately not updated here:
            //   UserId        - identifies which account owns this profile
            //   IsVerified    - a platform decision, not self-service
            //   AverageRating - denormalized from Review, recalculated after each review
            //   Balance       - money; only releasing an agreement escrow moves it

            await _context.SaveChangesAsync();
            return vendorProfile;
        }

        public async Task<bool> CreditBalanceAsync(int id, decimal amount)
        {
            var vendorProfile = await _context.VendorProfiles.FindAsync(id);
            if (vendorProfile is null) return false;

            vendorProfile.Balance += amount;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetAverageRatingAsync(int id, decimal? averageRating)
        {
            var vendorProfile = await _context.VendorProfiles.FindAsync(id);
            if (vendorProfile is null) return false;

            vendorProfile.AverageRating = averageRating;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var vendorProfile = await _context.VendorProfiles.FindAsync(id);
            if (vendorProfile is null) return false;

            // Throws DbUpdateException while the vendor still has offers or reviews.
            _context.VendorProfiles.Remove(vendorProfile);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
