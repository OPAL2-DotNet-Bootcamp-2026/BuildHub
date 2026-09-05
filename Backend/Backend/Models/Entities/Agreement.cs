using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models.Entities
{
    /// <summary>
    /// Created when an offer is accepted. Carries the payment state directly -
    /// there is no separate payment or escrow table.
    /// </summary>
    [Index(nameof(OfferId), IsUnique = true)]
    public class Agreement
    {
        [Key]
        public int AgreementId { get; set; }

        [ForeignKey(nameof(Offer))]
        public int OfferId { get; set; }

        [Precision(18, 3)]
        public decimal TotalAmount { get; set; }

        public AgreementStatus Status { get; set; }

        /// <summary>Mocked escrow: a status change plus an update to the vendor's balance.</summary>
        public PaymentStatus PaymentStatus { get; set; }

        public DateTime? HeldAt { get; set; }

        public DateTime? ReleasedAt { get; set; }

        public DateTime StartedAt { get; set; }

        // --- Relations ---

        /// <summary>1-1. The accepted offer this agreement was built from.</summary>
        [InverseProperty(nameof(Backend.Models.Entities.Offer.Agreement))]
        public Offer Offer { get; set; } = null!;

        /// <summary>1-M. Proof the work happened.</summary>
        [InverseProperty(nameof(Review.Agreement))]
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
