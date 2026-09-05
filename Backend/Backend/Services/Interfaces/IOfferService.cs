using Backend.Exceptions;
using Backend.Models.Dtos;

namespace Backend.Services.Interfaces
{
    public interface IOfferService
    {
        Task<IEnumerable<OfferResponse>> GetAllAsync();

        /// <summary>Null when no offer has this id.</summary>
        Task<OfferResponse?> GetByIdAsync(int id);

        /// <summary>
        /// Submits a Pending offer.
        /// Throws <see cref="NotFoundException"/> when the job or vendor does not exist, and
        /// <see cref="ConflictException"/> when the job is not Open or this vendor has
        /// already offered on it.
        /// </summary>
        Task<OfferResponse> CreateAsync(CreateOfferRequest request);

        /// <summary>
        /// Corrects a quote. Null when no offer has this id.
        /// Throws <see cref="ConflictException"/> once the offer is no longer Pending -
        /// the data model allows no revisions after that.
        /// </summary>
        Task<OfferResponse?> UpdateAsync(int id, UpdateOfferRequest request);

        /// <summary>
        /// False when no offer has this id.
        /// Throws <see cref="ConflictException"/> once it has been accepted into an agreement.
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}
