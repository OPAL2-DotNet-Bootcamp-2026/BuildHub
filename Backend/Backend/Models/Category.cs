using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    /// <summary>
    /// Flat list - no parent/child hierarchy.
    /// E.g. Kitchens, Ceramics, Electrical, Plumbing, Painting, Interior Design.
    /// </summary>
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string NameAr { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string NameEn { get; set; } = string.Empty;

        [Url]
        [MaxLength(500)]
        public string? IconUrl { get; set; }

        // --- Relations ---

        /// <summary>1-M. Vendors whose main trade this is.</summary>
        [InverseProperty(nameof(VendorProfile.Category))]
        public ICollection<VendorProfile> VendorProfiles { get; set; } = new List<VendorProfile>();

        /// <summary>1-M.</summary>
        [InverseProperty(nameof(Job.Category))]
        public ICollection<Job> Jobs { get; set; } = new List<Job>();

        /// <summary>1-M.</summary>
        [InverseProperty(nameof(Product.Category))]
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
