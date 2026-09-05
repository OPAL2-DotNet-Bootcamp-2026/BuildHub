using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Dtos
{
    /// <summary>
    /// Moves the escrow. Only the payment state is sent, because everything else
    /// follows from it: releasing completes the agreement and the job and credits the
    /// vendor, refunding cancels both. Sending Status separately would let the two
    /// disagree.
    /// </summary>
    public class UpdateAgreementRequest
    {
        [EnumDataType(typeof(PaymentStatus), ErrorMessage = "PaymentStatus must be Held, Released or Refunded.")]
        public PaymentStatus PaymentStatus { get; set; }
    }
}
