using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Entities
{
    /// <summary>
    /// Rating left after an agreement. All three FKs are required - every review is
    /// backed by a real completed job. There are no product reviews.
    /// </summary>
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        [ForeignKey(nameof(Reviewer))]
        public int ReviewerId { get; set; }

        [ForeignKey(nameof(VendorProfile))]
        public int VendorProfileId { get; set; }

        [ForeignKey(nameof(Agreement))]
        public int AgreementId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(2000)]
        public string? Comment { get; set; }

        public DateTime ReviewDate { get; set; }

        // --- Relations ---

        /// <summary>M-1. Must be the agreement's homeowner.</summary>
        [InverseProperty(nameof(User.Reviews))]
        public User Reviewer { get; set; } = null!;

        /// <summary>M-1. The vendor being rated.</summary>
        [InverseProperty(nameof(Backend.Models.Entities.VendorProfile.Reviews))]
        public VendorProfile VendorProfile { get; set; } = null!;

        /// <summary>M-1. Must be Completed before a review is allowed.</summary>
        [InverseProperty(nameof(Backend.Models.Entities.Agreement.Reviews))]
        public Agreement Agreement { get; set; } = null!;
    }
}
