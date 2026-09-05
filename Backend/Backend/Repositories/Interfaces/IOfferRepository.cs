using Backend.Models;
using Backend.Models.Entities;

namespace Backend.Repositories.Interfaces
{
    public interface IOfferRepository
    {
        Task<IEnumerable<Offer>> GetAllAsync();

        /// <summary>Returns null when no offer has this id.</summary>
        Task<Offer?> GetByIdAsync(int id);

        /// <summary>
        /// Throws DbUpdateException when this vendor already has an offer on this job:
        /// (JobId, VendorProfileId) is unique.
        /// </summary>
        Task<Offer> CreateAsync(Offer offer);

        /// <summary>
        /// Updates the terms of the offer. Status is not editable here: it moves only
        /// through the accept flow. Returns null when the id does not exist.
        /// </summary>
        Task<Offer?> UpdateAsync(int id, Offer input);

        /// <summary>Every offer on one job - used to check for duplicates and to
        /// mark the losing offers NotSelected when one is accepted.</summary>
        Task<IEnumerable<Offer>> GetByJobIdAsync(int jobId);

        /// <summary>
        /// Moves the offer along its state machine. Separate from <see cref="UpdateAsync"/>
        /// so an offer can never accept itself through an ordinary edit.
        /// False when the id does not exist.
        /// </summary>
        Task<bool> SetStatusAsync(int id, OfferStatus status);

        /// <summary>False when the id does not exist.</summary>
        Task<bool> DeleteAsync(int id);
    }
}
