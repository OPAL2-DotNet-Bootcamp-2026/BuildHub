using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Dtos
{
    /// <summary>
    /// Rates the vendor after a completed agreement. Neither party is named here:
    /// the vendor comes from the agreement, so a review cannot be filed against
    /// someone who did not do the work, and the reviewer comes from the token, so it
    /// cannot be written in another homeowner's name.
    /// </summary>
    public class CreateReviewRequest
    {
        [Range(1, int.MaxValue)]
        public int AgreementId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(2000)]
        public string? Comment { get; set; }
    }
}
