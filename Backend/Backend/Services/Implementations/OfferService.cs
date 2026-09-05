using Backend.Exceptions;
using Backend.Models.Dtos;
using Backend.Models;
using Backend.Models.Entities;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.Implementations
{
    public class OfferService : IOfferService
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IVendorProfileRepository _vendorProfileRepository;
        private readonly ICurrentUser _currentUser;

        public OfferService(
            IOfferRepository offerRepository,
            IJobRepository jobRepository,
            IVendorProfileRepository vendorProfileRepository,
            ICurrentUser currentUser)
        {
            _offerRepository = offerRepository;
            _jobRepository = jobRepository;
            _vendorProfileRepository = vendorProfileRepository;
            _currentUser = currentUser;
        }

        /// <summary>
        /// Only the offers you are party to: the ones you made as a vendor, and the
        /// ones sitting on your own jobs as a homeowner. A quote is commercially
        /// sensitive, so a signed-in competitor must not be able to read the whole
        /// board. An administrator sees everything.
        /// </summary>
        public async Task<IEnumerable<OfferResponse>> GetAllAsync()
        {
            var offers = await _offerRepository.GetAllAsync();

            if (_currentUser.IsAdmin)
            {
                return offers.Select(ToResponse);
            }

            var callerVendorProfileId = await GetCallerVendorProfileIdAsync();
            var callerJobIds = (await _jobRepository.GetAllAsync())
                .Where(job => job.HomeownerId == _currentUser.UserId)
                .Select(job => job.JobId)
                .ToHashSet();

            return offers
                .Where(offer => offer.VendorProfileId == callerVendorProfileId
                    || callerJobIds.Contains(offer.JobId))
                .Select(ToResponse);
        }

        public async Task<OfferResponse?> GetByIdAsync(int id)
        {
            var offer = await _offerRepository.GetByIdAsync(id);
            if (offer is null) return null;

            if (!await CanCallerSeeAsync(offer))
            {
                throw new ForbiddenException($"Offer {id} is not yours to view.");
            }

            return ToResponse(offer);
        }

        public async Task<OfferResponse> CreateAsync(CreateOfferRequest request)
        {
            var job = await _jobRepository.GetByIdAsync(request.JobId)
                ?? throw new NotFoundException($"No job with id {request.JobId}.");

            // "Offers can only be submitted while the Job is Open."
            if (job.Status != JobStatus.Open)
            {
                throw new ConflictException(
                    $"Job {request.JobId} is {job.Status}; offers can only be submitted while it is Open.");
            }

            // The offer comes from the caller's own vendor profile, never a body field.
            var vendorProfile = await GetCallerVendorProfileAsync();

            // "One offer per (vendorProfileId, jobId)." The unique index enforces it;
            // this check turns the violation into a clear 409.
            var offersOnJob = await _offerRepository.GetByJobIdAsync(request.JobId);
            if (offersOnJob.Any(o => o.VendorProfileId == vendorProfile.VendorProfileId))
            {
                throw new ConflictException(
                    $"Vendor {vendorProfile.VendorProfileId} has already made an offer on job {request.JobId}.");
            }

            try
            {
                var created = await _offerRepository.CreateAsync(new Offer
                {
                    JobId = request.JobId,
                    VendorProfileId = vendorProfile.VendorProfileId,
                    Price = request.Price,
                    DurationDays = request.DurationDays,
                    Message = request.Message,
                    // Step 2 of the flow: every new offer is Pending.
                    Status = OfferStatus.Pending,
                    SubmittedAt = DateTime.UtcNow
                });

                return ToResponse(created);
            }
            catch (DbUpdateException)
            {
                throw new ConflictException(
                    $"Vendor {vendorProfile.VendorProfileId} has already made an offer on job {request.JobId}.");
            }
        }

        public async Task<OfferResponse?> UpdateAsync(int id, UpdateOfferRequest request)
        {
            var offer = await _offerRepository.GetByIdAsync(id);
            if (offer is null) return null;

            await EnsureOwnedByCallerAsync(offer);

            // "No revisions, no counter-offers" - once the homeowner has decided, the
            // quote is part of the record and cannot move.
            if (offer.Status != OfferStatus.Pending)
            {
                throw new ConflictException(
                    $"Offer {id} is {offer.Status} and can no longer be changed.");
            }

            var updated = await _offerRepository.UpdateAsync(id, new Offer
            {
                Price = request.Price,
                DurationDays = request.DurationDays,
                Message = request.Message
            });

            return updated is null ? null : ToResponse(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var offer = await _offerRepository.GetByIdAsync(id);
            if (offer is null) return false;

            await EnsureOwnedByCallerAsync(offer);

            try
            {
                return await _offerRepository.DeleteAsync(id);
            }
            catch (DbUpdateException)
            {
                throw new ConflictException(
                    "This offer cannot be deleted because it has been accepted into an agreement.");
            }
        }

        /// <summary>
        /// Turns the signed-in user into the vendor they trade as. A Vendor-role
        /// account without a profile has nothing to offer from yet.
        /// </summary>
        private async Task<VendorProfile> GetCallerVendorProfileAsync()
        {
            return await _vendorProfileRepository.GetByUserIdAsync(_currentUser.UserId)
                ?? throw new ForbiddenException(
                    "This account has no vendor profile, so it cannot make offers.");
        }

        /// <summary>Null for a homeowner or admin, who have no profile of their own.</summary>
        private async Task<int?> GetCallerVendorProfileIdAsync() =>
            (await _vendorProfileRepository.GetByUserIdAsync(_currentUser.UserId))?.VendorProfileId;

        private async Task<bool> CanCallerSeeAsync(Offer offer)
        {
            if (_currentUser.IsAdmin) return true;

            if (offer.VendorProfileId == await GetCallerVendorProfileIdAsync()) return true;

            var job = await _jobRepository.GetByIdAsync(offer.JobId);
            return job is not null && job.HomeownerId == _currentUser.UserId;
        }

        private async Task EnsureOwnedByCallerAsync(Offer offer)
        {
            if (_currentUser.IsAdmin) return;

            var vendorProfile = await GetCallerVendorProfileAsync();
            if (offer.VendorProfileId != vendorProfile.VendorProfileId)
            {
                throw new ForbiddenException($"Offer {offer.OfferId} belongs to another vendor.");
            }
        }

        private static OfferResponse ToResponse(Offer offer) => new()
        {
            OfferId = offer.OfferId,
            JobId = offer.JobId,
            VendorProfileId = offer.VendorProfileId,
            Price = offer.Price,
            DurationDays = offer.DurationDays,
            Message = offer.Message,
            Status = offer.Status,
            SubmittedAt = offer.SubmittedAt
        };
    }
}
