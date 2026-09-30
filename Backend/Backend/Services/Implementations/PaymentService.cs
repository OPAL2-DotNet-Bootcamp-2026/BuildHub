using Backend.Data;
using Backend.Exceptions;
using Backend.Models;
using Backend.Models.Dtos;
using Backend.Models.Entities;
using Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.Implementations
{
    public class PaymentService
    {


        // BuildHub charges a 2% fee
        private const decimal BuildHubFeeRate = 0.02m;

        private readonly BuildHubDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly ThawaniGateway _thawaniGateway;

        public PaymentService(
            BuildHubDbContext context,
            ICurrentUser currentUser,
            ThawaniGateway thawaniGateway)
        {
            _context = context;
            _currentUser = currentUser;
            _thawaniGateway = thawaniGateway;
        }

        // Create the Thawani checkout
        public async Task<PaymentCheckoutResponse> CreateCheckoutAsync(
            int agreementId)
        {
            // Get the agreement and homeowner information
            var agreement = await _context.Agreements
                .Include(a => a.Offer)
                .ThenInclude(o => o.Job)
                .ThenInclude(j => j.Homeowner)
                .FirstOrDefaultAsync(
                    a => a.AgreementId == agreementId);

            if (agreement is null)
            {
                throw new NotFoundException(
                    $"No agreement with ID {agreementId}.");
            }

            var homeowner = agreement.Offer.Job.Homeowner;

            // Only the homeowner who owns the agreement can pay
            if (homeowner.UserId != _currentUser.UserId)
            {
                throw new ForbiddenException(
                    "This agreement belongs to another homeowner.");
            }

            // The payment must still be Pending
            if (agreement.PaymentStatus != PaymentStatus.Pending)
            {
                throw new ConflictException(
                    $"Payment is already {agreement.PaymentStatus}.");
            }

            // Check if a checkout session already exists
            var existingPayment = await _context.Payments
                .Where(p =>
                    p.AgreementId == agreementId
                    && p.Status == GatewayPaymentStatus.Pending
                    && p.CheckoutUrl != null)
                .OrderByDescending(p => p.PaymentId)
                .FirstOrDefaultAsync();

            // Reuse the existing checkout
            if (existingPayment is not null)
            {
                return new PaymentCheckoutResponse
                {
                    PaymentId = existingPayment.PaymentId,
                    CheckoutUrl = existingPayment.CheckoutUrl!
                };
            }

            // Calculate BuildHub's 2% fee
            var buildHubFee = decimal.Round(
                agreement.TotalAmount * BuildHubFeeRate,
                3,
                MidpointRounding.AwayFromZero);

            var totalAmount =
                agreement.TotalAmount + buildHubFee;

            // Create a new payment record
            var payment = new Payment
            {
                AgreementId = agreement.AgreementId,

                HomeownerId = homeowner.UserId,

                AgreementAmount = agreement.TotalAmount,

                BuildHubFee = buildHubFee,

                TotalAmount = totalAmount,

                Currency = "OMR",

                Status = GatewayPaymentStatus.Pending,

                ClientReferenceId =
                    $"pending-{Guid.NewGuid():N}",

                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);

            await _context.SaveChangesAsync();

            // PaymentId is available after SaveChanges
            payment.ClientReferenceId =
                $"buildhub-payment-{payment.PaymentId}";

            try
            {
                // Thawani requires the amount in baisa
                // 1 OMR = 1000 baisa
                var amountInBaisa = checked(
                    (long)decimal.Round(
                        totalAmount * 1000m,
                        0,
                        MidpointRounding.AwayFromZero));

                // Information sent to Thawani metadata
                var metadata =
                    new Dictionary<string, string>
                    {
                        ["Customer name"] =
                            homeowner.FullName,

                        ["Contact number"] =
                            homeowner.PhoneNumber ?? "",

                        ["Email address"] =
                            homeowner.Email,

                        ["agreement_id"] =
                            agreement.AgreementId.ToString(),

                        ["payment_id"] =
                            payment.PaymentId.ToString()
                    };

                // Create the checkout with Thawani
                var checkout =
                    await _thawaniGateway.CreateCheckoutAsync(
                        payment.ClientReferenceId,
                        $"BuildHub Agreement #{agreement.AgreementId}",
                        amountInBaisa,
                        metadata);

                // Save the values returned by Thawani
                payment.GatewaySessionId =
                    checkout.SessionId;

                payment.CheckoutUrl =
                    checkout.CheckoutUrl;

                await _context.SaveChangesAsync();

                // Return the checkout URL to the frontend
                return new PaymentCheckoutResponse
                {
                    PaymentId = payment.PaymentId,

                    CheckoutUrl = checkout.CheckoutUrl
                };
            }
            catch
            {
                // Store the failed payment attempt
                payment.Status =
                    GatewayPaymentStatus.Failed;

                await _context.SaveChangesAsync();

                throw;
            }
        }

        // Verify the payment after Thawani redirects back
        public async Task<PaymentVerificationResponse> VerifyAsync(
            int paymentId)
        {
            var payment = await _context.Payments
                .Include(p => p.Agreement)
                .FirstOrDefaultAsync(
                    p => p.PaymentId == paymentId);

            if (payment is null)
            {
                throw new NotFoundException(
                    $"No payment with ID {paymentId}.");
            }

            // Make sure the payment belongs to the logged-in homeowner
            if (payment.HomeownerId != _currentUser.UserId)
            {
                throw new ForbiddenException(
                    "This payment belongs to another homeowner.");
            }

            // The payment was already verified
            if (payment.Status == GatewayPaymentStatus.Held)
            {
                return new PaymentVerificationResponse
                {
                    Status = payment.Status,

                    Message = "Payment was already verified."
                };
            }

            if (string.IsNullOrWhiteSpace(
                    payment.GatewaySessionId))
            {
                throw new ConflictException(
                    "This payment does not have a Thawani session.");
            }

            // Get the real payment status from Thawani
            var session =
                await _thawaniGateway.GetSessionAsync(
                    payment.GatewaySessionId);

            switch (session.PaymentStatus
                .Trim()
                .ToLowerInvariant())
            {
                case "paid":
                    payment.Status =
                        GatewayPaymentStatus.Held;

                    payment.GatewayPaymentId =
                        session.PaymentId;

                    payment.PaidAt =
                        DateTime.UtcNow;

                    // Change the agreement from Pending to Held
                    payment.Agreement.PaymentStatus =
                        PaymentStatus.Held;

                    payment.Agreement.HeldAt =
                        payment.PaidAt;
                    break;

                case "cancelled":
                case "canceled":
                    payment.Status =
                        GatewayPaymentStatus.Cancelled;
                    break;

                case "failed":
                    payment.Status =
                        GatewayPaymentStatus.Failed;
                    break;

                default:
                    payment.Status =
                        GatewayPaymentStatus.Pending;
                    break;
            }

            await _context.SaveChangesAsync();

            var message =
                payment.Status == GatewayPaymentStatus.Held
                    ? "Payment verified and marked as held."
                    : $"Thawani payment status: {session.PaymentStatus}.";

            return new PaymentVerificationResponse
            {
                Status = payment.Status,

                Message = message
            };
        }
    }
}
