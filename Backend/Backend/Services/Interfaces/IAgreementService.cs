using Backend.Exceptions;
using Backend.Models.Dtos;

namespace Backend.Services.Interfaces
{
    public interface IAgreementService
    {
        Task<IEnumerable<AgreementResponse>> GetAllAsync();

        /// <summary>Null when no agreement has this id.</summary>
        Task<AgreementResponse?> GetByIdAsync(int id);

        /// <summary>
        /// Accepts an offer. In one transaction: that offer becomes Accepted, every
        /// other offer on the job becomes NotSelected, the job becomes Hired, and a new
        /// agreement is created Active with its payment Held.
        /// Throws <see cref="NotFoundException"/> when the offer does not exist, and
        /// <see cref="ConflictException"/> when it is not Pending or its job is not Open.
        /// </summary>
        Task<AgreementResponse> CreateAsync(CreateAgreementRequest request);

        /// <summary>
        /// Moves the escrow, in one transaction. Releasing completes the agreement and
        /// its job and credits the vendor's balance; refunding cancels both.
        /// Null when no agreement has this id.
        /// Throws <see cref="ConflictException"/> when the payment has already been
        /// released or refunded.
        /// </summary>
        Task<AgreementResponse?> UpdateAsync(int id, UpdateAgreementRequest request);

        /// <summary>False when no agreement has this id.</summary>
        Task<bool> DeleteAsync(int id);
    }
}
