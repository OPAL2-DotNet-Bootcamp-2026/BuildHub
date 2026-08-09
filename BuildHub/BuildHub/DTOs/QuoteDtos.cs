using System.ComponentModel.DataAnnotations;

namespace BuildHub.DTOs
{
    public class QuoteInputDto
    {
        [Required]
        public int VendorProfileId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public int DurationDays { get; set; }
    }


    // Quote Output
    public class QuoteOutputDto
    {
        public int QuoteId { get; set; }
        public int QuoteRequestId { get; set; }
        public int VendorProfileId { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public string Status { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}
