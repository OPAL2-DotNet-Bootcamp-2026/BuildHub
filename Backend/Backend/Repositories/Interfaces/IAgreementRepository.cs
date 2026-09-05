using Backend.Models;
using Backend.Models.Entities;

namespace Backend.Repositories.Interfaces
{
    public interface IAgreementRepository
    {
        Task<IEnumerable<Agreement>> GetAllAsync();

        /// <summary>Returns null when no agreement has this id.</summary>
        Task<Agreement?> GetByIdAsync(int id);

        /// <summary>
        /// Throws DbUpdateException when the offer already has an agreement:
        /// OfferId is unique, so only the accepted offer ever gets one.
        /// </summary>
        Task<Agreement> CreateAsync(Agreement agreement);

        /// <summary>
        /// Updates the escrow state machine and its timestamps. Returns null when the
        /// id does not exist. Crediting VendorProfile.Balance on release is a service
        /// concern, not this repository.
        /// </summary>
        Task<Agreement?> UpdateAsync(int id, Agreement input);

        /// <summary>False when the id does not exist.</summary>
        Task<bool> DeleteAsync(int id);
    }
}
