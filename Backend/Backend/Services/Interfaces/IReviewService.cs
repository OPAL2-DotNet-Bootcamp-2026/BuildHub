using Backend.Exceptions;
using Backend.Models.Dtos;

namespace Backend.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewResponse>> GetAllAsync();

        /// <summary>Null when no review has this id.</summary>
        Task<ReviewResponse?> GetByIdAsync(int id);

        /// <summary>
        /// Rates the vendor behind a completed agreement, then recomputes their average.
        /// Throws <see cref="NotFoundException"/> when the agreement or reviewer does not
        /// exist, <see cref="ConflictException"/> when the agreement is not Completed, and
        /// <see cref="BadRequestException"/> when the reviewer is not that agreement's
        /// homeowner.
        /// </summary>
        Task<ReviewResponse> CreateAsync(CreateReviewRequest request);

        /// <summary>
        /// Changes the rating or comment and recomputes the vendor's average.
        /// Null when no review has this id.
        /// </summary>
        Task<ReviewResponse?> UpdateAsync(int id, UpdateReviewRequest request);

        /// <summary>
        /// False when no review has this id. Recomputes the vendor's average afterwards.
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}
