using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Dtos
{
    /// <summary>
    /// Opens the business layer over an existing Vendor-role account.
    /// IsVerified, AverageRating and Balance are absent: a vendor cannot arrive
    /// pre-verified, pre-rated or pre-funded.
    /// </summary>
    public class CreateVendorProfileRequest
    {
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required]
        [MaxLength(150)]
        public string CompanyName { get; set; } = string.Empty;

        [EnumDataType(typeof(VendorType), ErrorMessage = "VendorType must be Contractor, Designer or Store.")]
        public VendorType VendorType { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(80)]
        public string City { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Bio { get; set; }
    }
}
