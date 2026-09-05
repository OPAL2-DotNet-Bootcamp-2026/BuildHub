using Backend.Exceptions;
using Backend.Models.Dtos;
using Backend.Models;
using Backend.Models.Entities;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;

namespace Backend.Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IAgreementRepository _agreementRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVendorProfileRepository _vendorProfileRepository;
        private readonly ICurrentUser _currentUser;

        public ReviewService(
            IReviewRepository reviewRepository,
            IAgreementRepository agreementRepository,
            IOfferRepository offerRepository,
            IJobRepository jobRepository,
            IUserRepository userRepository,
            IVendorProfileRepository vendorProfileRepository,
            ICurrentUser currentUser)
        {
            _currentUser = currentUser;
            _reviewRepository = reviewRepository;
            _agreementRepository = agreementRepository;
            _offerRepository = offerRepository;
            _jobRepository = jobRepository;
            _userRepository = userRepository;
            _vendorProfileRepository = vendorProfileRepository;
        }

        public async Task<IEnumerable<ReviewResponse>> GetAllAsync()
        {
            var reviews = await _reviewRepository.GetAllAsync();
            return reviews.Select(ToResponse);
        }

        public async Task<ReviewResponse?> GetByIdAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            return review is null ? null : ToResponse(review);
        }

        public async Task<ReviewResponse> CreateAsync(CreateReviewRequest request)
        {
            var agreement = await _agreementRepository.GetByIdAsync(request.AgreementId)
                ?? throw new NotFoundException($"No agreement with id {request.AgreementId}.");

            // "A Review requires a Completed Agreement."
            if (agreement.Status != AgreementStatus.Completed)
            {
                throw new ConflictException(
                    $"Agreement {request.AgreementId} is {agreement.Status}; only a Completed agreement can be reviewed.");
            }

            // The reviewer is the signed-in caller, never a body field.
            var reviewerId = _currentUser.UserId;

            if (await _userRepository.GetByIdAsync(reviewerId) is null)
            {
                throw new NotFoundException($"No user with id {reviewerId}.");
            }

            // Walk Agreement -> Offer -> Job to find both who may review and who is
            // being reviewed. Neither is taken from the request, so a review cannot be
            // filed by a stranger or against a vendor who did not do the work.
            var offer = await _offerRepository.GetByIdAsync(agreement.OfferId)
                ?? throw new NotFoundException($"No offer with id {agreement.OfferId}.");
            var job = await _jobRepository.GetByIdAsync(offer.JobId)
                ?? throw new NotFoundException($"No job with id {offer.JobId}.");

            // "The reviewer must be that agreement's homeowner." Now that the reviewer
            // comes from the token, this is a real ownership check rather than a
            // consistency check on two numbers the caller supplied.
            if (job.HomeownerId != reviewerId)
            {
                throw new ForbiddenException(
                    $"User {reviewerId} is not the homeowner of agreement {request.AgreementId}.");
            }

            var created = await _reviewRepository.CreateAsync(new Review
            {
                ReviewerId = reviewerId,
                VendorProfileId = offer.VendorProfileId,
                AgreementId = request.AgreementId,
                Rating = request.Rating,
                Comment = request.Comment,
                ReviewDate = DateTime.UtcNow
            });

            // Step 6 of the flow.
            await RecalculateAverageRatingAsync(offer.VendorProfileId);

            return ToResponse(created);
        }

        public async Task<ReviewResponse?> UpdateAsync(int id, UpdateReviewRequest request)
        {
            var existing = await _reviewRepository.GetByIdAsync(id);
            if (existing is null) return null;

            EnsureWrittenByCaller(existing);

            var updated = await _reviewRepository.UpdateAsync(id, new Review
            {
                Rating = request.Rating,
                Comment = request.Comment
            });

            if (updated is null) return null;

            await RecalculateAverageRatingAsync(updated.VendorProfileId);
            return ToResponse(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // Read it first: once it is gone we can no longer tell whose average moved.
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review is null) return false;

            EnsureWrittenByCaller(review);

            var deleted = await _reviewRepository.DeleteAsync(id);
            if (deleted)
            {
                await RecalculateAverageRatingAsync(review.VendorProfileId);
            }

            return deleted;
        }

        private void EnsureWrittenByCaller(Review review)
        {
            if (!_currentUser.IsAdmin && review.ReviewerId != _currentUser.UserId)
            {
                throw new ForbiddenException($"Review {review.ReviewId} was written by someone else.");
            }
        }

        /// <summary>
        /// Recomputes the denormalized VendorProfile.AverageRating from the reviews that
        /// remain. Null when there are none, matching a vendor who has never been rated.
        /// </summary>
        private async Task RecalculateAverageRatingAsync(int vendorProfileId)
        {
            var reviews = (await _reviewRepository.GetByVendorProfileIdAsync(vendorProfileId)).ToList();

            decimal? average = reviews.Count == 0
                ? null
                : Math.Round(reviews.Average(r => (decimal)r.Rating), 2, MidpointRounding.AwayFromZero);

            await _vendorProfileRepository.SetAverageRatingAsync(vendorProfileId, average);
        }

        private static ReviewResponse ToResponse(Review review) => new()
        {
            ReviewId = review.ReviewId,
            ReviewerId = review.ReviewerId,
            VendorProfileId = review.VendorProfileId,
            AgreementId = review.AgreementId,
            Rating = review.Rating,
            Comment = review.Comment,
            ReviewDate = review.ReviewDate
        };
    }
}
