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

        public OfferService(
            IOfferRepository offerRepository,
            IJobRepository jobRepository,
            IVendorProfileRepository vendorProfileRepository)
        {
            _offerRepository = offerRepository;
            _jobRepository = jobRepository;
            _vendorProfileRepository = vendorProfileRepository;
        }

        public async Task<IEnumerable<OfferResponse>> GetAllAsync()
        {
            var offers = await _offerRepository.GetAllAsync();
            return offers.Select(ToResponse);
        }

        public async Task<OfferResponse?> GetByIdAsync(int id)
        {
            var offer = await _offerRepository.GetByIdAsync(id);
            return offer is null ? null : ToResponse(offer);
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

            if (await _vendorProfileRepository.GetByIdAsync(request.VendorProfileId) is null)
            {
                throw new NotFoundException($"No vendor profile with id {request.VendorProfileId}.");
            }

            // "One offer per (vendorProfileId, jobId)." The unique index enforces it;
            // this check turns the violation into a clear 409.
            var offersOnJob = await _offerRepository.GetByJobIdAsync(request.JobId);
            if (offersOnJob.Any(o => o.VendorProfileId == request.VendorProfileId))
            {
                throw new ConflictException(
                    $"Vendor {request.VendorProfileId} has already made an offer on job {request.JobId}.");
            }

            try
            {
                var created = await _offerRepository.CreateAsync(new Offer
                {
                    JobId = request.JobId,
                    VendorProfileId = request.VendorProfileId,
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
                    $"Vendor {request.VendorProfileId} has already made an offer on job {request.JobId}.");
            }
        }

        public async Task<OfferResponse?> UpdateAsync(int id, UpdateOfferRequest request)
        {
            var offer = await _offerRepository.GetByIdAsync(id);
            if (offer is null) return null;

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
