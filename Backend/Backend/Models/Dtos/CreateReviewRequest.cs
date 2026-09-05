using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Dtos
{
    /// <summary>
    /// Rates the vendor after a completed agreement. VendorProfileId is absent on
    /// purpose: the service reads it from the agreement, so a review can never be
    /// filed against a vendor who did not do the work.
    /// </summary>
    public class CreateReviewRequest
    {
        [Range(1, int.MaxValue)]
        public int ReviewerId { get; set; }

        [Range(1, int.MaxValue)]
        public int AgreementId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(2000)]
        public string? Comment { get; set; }
    }
}
