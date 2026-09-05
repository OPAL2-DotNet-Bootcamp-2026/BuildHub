using Backend.Data;
using Backend.Models;
using Backend.Models.Entities;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories.Implementations
{
    public class AgreementRepository : IAgreementRepository
    {
        private readonly BuildHubDbContext _context;

        public AgreementRepository(BuildHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Agreement>> GetAllAsync() =>
            await _context.Agreements.AsNoTracking().ToListAsync();

        public async Task<Agreement?> GetByIdAsync(int id) =>
            await _context.Agreements.AsNoTracking()
                .FirstOrDefaultAsync(a => a.AgreementId == id);

        public async Task<Agreement> CreateAsync(Agreement agreement)
        {
            _context.Agreements.Add(agreement);
            await _context.SaveChangesAsync();
            return agreement;
        }

        public async Task<Agreement?> UpdateAsync(int id, Agreement input)
        {
            var agreement = await _context.Agreements.FindAsync(id);
            if (agreement is null) return null;

            // The escrow state machine and the timestamps that record it:
            // Active/Held -> Completed/Released, or Refunded when an admin steps in.
            agreement.Status = input.Status;
            agreement.PaymentStatus = input.PaymentStatus;
            agreement.HeldAt = input.HeldAt;
            agreement.ReleasedAt = input.ReleasedAt;

            // Deliberately not updated here:
            //   OfferId     - the 1-1 link this agreement was built from
            //   TotalAmount - copied from the accepted offer price; repricing after the
            //                 fact would change what was actually agreed and escrowed
            //   StartedAt   - a historical fact

            await _context.SaveChangesAsync();
            return agreement;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var agreement = await _context.Agreements.FindAsync(id);
            if (agreement is null) return false;

            // Cascades to this agreement's reviews.
            _context.Agreements.Remove(agreement);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
