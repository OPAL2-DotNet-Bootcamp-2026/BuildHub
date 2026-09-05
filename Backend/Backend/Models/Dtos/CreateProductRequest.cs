using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Dtos
{
    /// <summary>
    /// Lists a material item. VendorProfileId is absent: the listing belongs to the
    /// signed-in vendor's own profile.
    /// </summary>
    public class CreateProductRequest
    {
        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [EnumDataType(typeof(ProductUnit), ErrorMessage = "Unit must be SquareMeter, Piece or Set.")]
        public ProductUnit Unit { get; set; }

        [Range(0, 9999999999999.999)]
        public decimal Price { get; set; }

        [Url]
        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}
