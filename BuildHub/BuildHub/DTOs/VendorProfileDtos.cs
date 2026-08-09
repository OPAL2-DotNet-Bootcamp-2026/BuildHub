using BuildHub.Enums;

namespace BuildHub.DTOs
{
    public class VendorProfileResponseDto
    {
        public int VendorProfileId { get; set; }
        public int UserId { get; set; }
        public string CompanyName { get; set; }
        public VendorType VendorType { get; set; }
        public string City { get; set; }
        public bool IsVerified { get; set; }
        public decimal? AverageRating { get; set; }
        public decimal Balance { get; set; }
    }
}
