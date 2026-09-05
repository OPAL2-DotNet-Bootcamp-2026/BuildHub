using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Dtos
{
    public class CreateProductRequest
    {
        [Range(1, int.MaxValue)]
        public int VendorProfileId { get; set; }

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
