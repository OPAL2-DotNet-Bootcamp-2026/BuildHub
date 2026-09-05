namespace Backend.Models.Dtos
{
    public class ReviewResponse
    {
        public int ReviewId { get; set; }

        public int ReviewerId { get; set; }

        public int VendorProfileId { get; set; }

        public int AgreementId { get; set; }

        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime ReviewDate { get; set; }
    }
}
