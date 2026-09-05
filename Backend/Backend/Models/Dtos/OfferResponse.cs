
namespace Backend.Models.Dtos
{
    public class OfferResponse
    {
        public int OfferId { get; set; }

        public int JobId { get; set; }

        public int VendorProfileId { get; set; }

        public decimal Price { get; set; }

        public int DurationDays { get; set; }

        public string? Message { get; set; }

        public OfferStatus Status { get; set; }

        public DateTime SubmittedAt { get; set; }
    }
}
