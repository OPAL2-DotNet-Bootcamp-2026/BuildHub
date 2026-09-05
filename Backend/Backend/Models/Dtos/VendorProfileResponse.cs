
namespace Backend.Models.Dtos
{
    public class VendorProfileResponse
    {
        public int VendorProfileId { get; set; }

        public int UserId { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public VendorType VendorType { get; set; }

        public int CategoryId { get; set; }

        public string City { get; set; } = string.Empty;

        public string? Bio { get; set; }

        public bool IsVerified { get; set; }

        /// <summary>Recomputed from this vendor's reviews; null until they have one.</summary>
        public decimal? AverageRating { get; set; }

        /// <summary>Earnings wallet, credited when an agreement's escrow is released.</summary>
        public decimal Balance { get; set; }
    }
}
