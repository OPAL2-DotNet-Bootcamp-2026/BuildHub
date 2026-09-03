using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models
{
    /// <summary>
    /// A material item a store sells. For price comparison only - never linked to
    /// jobs, offers or agreements. No cart, no checkout.
    /// </summary>
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [ForeignKey(nameof(VendorProfile))]
        public int VendorProfileId { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        public ProductUnit Unit { get; set; }

        [Precision(18, 3)]
        public decimal Price { get; set; }

        [Url]
        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        public bool IsAvailable { get; set; }

        // --- Relations ---

        /// <summary>M-1. The store listing this product.</summary>
        [InverseProperty(nameof(Models.VendorProfile.Products))]
        public VendorProfile VendorProfile { get; set; } = null!;

        /// <summary>M-1.</summary>
        [InverseProperty(nameof(Models.Category.Products))]
        public Category Category { get; set; } = null!;
    }
}
