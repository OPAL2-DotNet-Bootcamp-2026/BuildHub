using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models.Entities
{
    /// <summary>
    /// A vendor's response to a job. No revisions, no counter-offers.
    /// </summary>
    [Index(nameof(JobId), nameof(VendorProfileId), IsUnique = true)] // one offer per vendor per job
    public class Offer
    {
        [Key]
        public int OfferId { get; set; }

        [ForeignKey(nameof(Job))]
        public int JobId { get; set; }

        [ForeignKey(nameof(VendorProfile))]
        public int VendorProfileId { get; set; }

        [Precision(18, 3)]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue)]
        public int DurationDays { get; set; }

        [MaxLength(2000)]
        public string? Message { get; set; }

        public OfferStatus Status { get; set; }

        public DateTime SubmittedAt { get; set; }

        // --- Relations ---

        /// <summary>M-1.</summary>
        [InverseProperty(nameof(Backend.Models.Entities.Job.Offers))]
        public Job Job { get; set; } = null!;

        /// <summary>M-1.</summary>
        [InverseProperty(nameof(Backend.Models.Entities.VendorProfile.Offers))]
        public VendorProfile VendorProfile { get; set; } = null!;

        /// <summary>1-1. Only the Accepted offer ever gets one.</summary>
        [InverseProperty(nameof(Backend.Models.Entities.Agreement.Offer))]
        public Agreement? Agreement { get; set; }
    }
}
