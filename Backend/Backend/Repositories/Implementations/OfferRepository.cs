using Backend.Data;
using Backend.Models;
using Backend.Models.Entities;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories.Implementations
{
    public class OfferRepository : IOfferRepository
    {
        private readonly BuildHubDbContext _context;

        public OfferRepository(BuildHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Offer>> GetAllAsync() =>
            await _context.Offers.AsNoTracking().ToListAsync();

        public async Task<Offer?> GetByIdAsync(int id) =>
            await _context.Offers.AsNoTracking().FirstOrDefaultAsync(o => o.OfferId == id);

        public async Task<Offer> CreateAsync(Offer offer)
        {
            _context.Offers.Add(offer);
            await _context.SaveChangesAsync();
            return offer;
        }

        public async Task<Offer?> UpdateAsync(int id, Offer input)
        {
            var offer = await _context.Offers.FindAsync(id);
            if (offer is null) return null;

            // The terms the vendor quoted.
            // Note: the data model says "no revisions, no counter-offers", so the
            // service layer should allow this only while Status is Pending.
            offer.Price = input.Price;
            offer.DurationDays = input.DurationDays;
            offer.Message = input.Message;

            // Deliberately not updated here:
            //   JobId, VendorProfileId - the unique pair identifying this offer
            //   Status                 - a state machine: accepting one offer sets it to
            //                            Accepted and every sibling to NotSelected
            //   SubmittedAt            - a historical fact

            await _context.SaveChangesAsync();
            return offer;
        }

        public async Task<IEnumerable<Offer>> GetByJobIdAsync(int jobId) =>
            await _context.Offers.AsNoTracking().Where(o => o.JobId == jobId).ToListAsync();

        public async Task<bool> SetStatusAsync(int id, OfferStatus status)
        {
            var offer = await _context.Offers.FindAsync(id);
            if (offer is null) return false;

            offer.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var offer = await _context.Offers.FindAsync(id);
            if (offer is null) return false;

            // Throws DbUpdateException once the offer has been accepted into an
            // agreement: that relation is Restrict, so escrowed money is never orphaned.
            _context.Offers.Remove(offer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
