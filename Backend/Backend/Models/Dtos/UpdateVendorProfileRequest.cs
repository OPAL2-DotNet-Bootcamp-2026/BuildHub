using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Dtos
{
    public class UpdateVendorProfileRequest
    {
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
