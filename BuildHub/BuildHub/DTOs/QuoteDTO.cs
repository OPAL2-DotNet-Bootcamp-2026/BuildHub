using System.ComponentModel.DataAnnotations;

namespace BuildHub.DTOs
{
    public class QuoteInputDTO
    {
        [Required]
        public int vendorProfileId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal price { get; set; }

        [Required]
        public int durationDays { get; set; }
    }

    public class QuoteOutputDTO
    {
        public int quoteId { get; set; }
        public int quoteRequestId { get; set; }
        public int vendorProfileId { get; set; }
        public decimal price { get; set; }
        public int durationDays { get; set; }
        public string status { get; set; }
        public DateTime submittedAt { get; set; }
    }
}