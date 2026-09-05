using Backend.Data;
using Backend.Exceptions;
using Backend.Models.Dtos;
using Backend.Models;
using Backend.Models.Entities;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.Implementations
{
    public class AgreementService : IAgreementService
    {
        private readonly IAgreementRepository _agreementRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IVendorProfileRepository _vendorProfileRepository;

        // The context is injected only to open a transaction. Every repository shares
        // this same scoped instance, so their individual SaveChanges calls all enlist
        // in it and the multi-step flows below commit or roll back as one.
        private readonly BuildHubDbContext _context;

        public AgreementService(
            IAgreementRepository agreementRepository,
            IOfferRepository offerRepository,
            IJobRepository jobRepository,
            IVendorProfileRepository vendorProfileRepository,
            BuildHubDbContext context)
        {
            _agreementRepository = agreementRepository;
            _offerRepository = offerRepository;
            _jobRepository = jobRepository;
            _vendorProfileRepository = vendorProfileRepository;
            _context = context;
        }

        public async Task<IEnumerable<AgreementResponse>> GetAllAsync()
        {
            var agreements = await _agreementRepository.GetAllAsync();
            return agreements.Select(ToResponse);
        }

        public async Task<AgreementResponse?> GetByIdAsync(int id)
        {
            var agreement = await _agreementRepository.GetByIdAsync(id);
            return agreement is null ? null : ToResponse(agreement);
        }

        public async Task<AgreementResponse> CreateAsync(CreateAgreementRequest request)
        {
            var offer = await _offerRepository.GetByIdAsync(request.OfferId)
                ?? throw new NotFoundException($"No offer with id {request.OfferId}.");

            if (offer.Status != OfferStatus.Pending)
            {
                throw new ConflictException(
                    $"Offer {request.OfferId} is {offer.Status}; only a Pending offer can be accepted.");
            }

            var job = await _jobRepository.GetByIdAsync(offer.JobId)
                ?? throw new NotFoundException($"No job with id {offer.JobId}.");

            if (job.Status != JobStatus.Open)
            {
                throw new ConflictException(
                    $"Job {job.JobId} is {job.Status}; only an Open job can hire a vendor.");
            }

            // Step 3 of the flow. All four changes succeed together or none do.
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var sibling in await _offerRepository.GetByJobIdAsync(job.JobId))
                {
                    if (sibling.OfferId != offer.OfferId && sibling.Status == OfferStatus.Pending)
                    {
                        await _offerRepository.SetStatusAsync(sibling.OfferId, OfferStatus.NotSelected);
                    }
                }

                await _offerRepository.SetStatusAsync(offer.OfferId, OfferStatus.Accepted);
                await _jobRepository.SetStatusAsync(job.JobId, JobStatus.Hired);

                var now = DateTime.UtcNow;
                var created = await _agreementRepository.CreateAsync(new Agreement
                {
                    OfferId = offer.OfferId,
                    // Copied from the accepted price so the two can never disagree.
                    TotalAmount = offer.Price,
                    Status = AgreementStatus.Active,
                    PaymentStatus = PaymentStatus.Held,
                    HeldAt = now,
                    StartedAt = now
                });

                await transaction.CommitAsync();
                return ToResponse(created);
            }
            catch (DbUpdateException)
            {
                await transaction.RollbackAsync();
                throw new ConflictException(
                    $"Offer {request.OfferId} has already been accepted into an agreement.");
            }
        }

        public async Task<AgreementResponse?> UpdateAsync(int id, UpdateAgreementRequest request)
        {
            var agreement = await _agreementRepository.GetByIdAsync(id);
            if (agreement is null) return null;

            if (request.PaymentStatus == agreement.PaymentStatus)
            {
                return ToResponse(agreement);
            }

            // Held is the only state anything can move out of; Released and Refunded
            // are terminal. Money never moves twice.
            if (agreement.PaymentStatus != PaymentStatus.Held)
            {
                throw new ConflictException(
                    $"Agreement {id} is already {agreement.PaymentStatus} and cannot change.");
            }

            var offer = await _offerRepository.GetByIdAsync(agreement.OfferId)
                ?? throw new NotFoundException($"No offer with id {agreement.OfferId}.");

            await using var transaction = await _context.Database.BeginTransactionAsync();

            Agreement input;
            if (request.PaymentStatus == PaymentStatus.Released)
            {
                // Step 5 of the flow: release the escrow, credit the vendor, close the job.
                await _vendorProfileRepository.CreditBalanceAsync(
                    offer.VendorProfileId, agreement.TotalAmount);
                await _jobRepository.SetStatusAsync(offer.JobId, JobStatus.Completed);

                input = new Agreement
                {
                    Status = AgreementStatus.Completed,
                    PaymentStatus = PaymentStatus.Released,
                    HeldAt = agreement.HeldAt,
                    ReleasedAt = DateTime.UtcNow
                };
            }
            else
            {
                // The refund path: an admin unwinds the deal. No balance is credited,
                // because nothing was ever earned.
                await _jobRepository.SetStatusAsync(offer.JobId, JobStatus.Cancelled);

                input = new Agreement
                {
                    Status = AgreementStatus.Cancelled,
                    PaymentStatus = PaymentStatus.Refunded,
                    HeldAt = agreement.HeldAt,
                    ReleasedAt = null
                };
            }

            var updated = await _agreementRepository.UpdateAsync(id, input);
            await transaction.CommitAsync();

            return updated is null ? null : ToResponse(updated);
        }

        public async Task<bool> DeleteAsync(int id) =>
            await _agreementRepository.DeleteAsync(id);

        private static AgreementResponse ToResponse(Agreement agreement) => new()
        {
            AgreementId = agreement.AgreementId,
            OfferId = agreement.OfferId,
            TotalAmount = agreement.TotalAmount,
            Status = agreement.Status,
            PaymentStatus = agreement.PaymentStatus,
            HeldAt = agreement.HeldAt,
            ReleasedAt = agreement.ReleasedAt,
            StartedAt = agreement.StartedAt
        };
    }
}
