
namespace Backend.Models.Dtos
{
    public class ProductResponse
    {
        public int ProductId { get; set; }

        public int VendorProfileId { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; } = string.Empty;

        public ProductUnit Unit { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsAvailable { get; set; }
    }
}
