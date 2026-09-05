using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models.Entities
{
    /// <summary>
    /// The business layer over a Vendor-role user. One per vendor user.
    /// </summary>
    [Index(nameof(UserId), IsUnique = true)]
    public class VendorProfile
    {
        [Key]
        public int VendorProfileId { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Required]
        [MaxLength(150)]
        public string CompanyName { get; set; } = string.Empty;

        public VendorType VendorType { get; set; }

        /// <summary>The vendor's main trade.</summary>
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(80)]
        public string City { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Bio { get; set; }

        public bool IsVerified { get; set; }

        /// <summary>Denormalized from <see cref="Review"/>; recalculated on every new review.</summary>
        [Precision(3, 2)]
        public decimal? AverageRating { get; set; }

        /// <summary>Earnings wallet, credited when an agreement's payment is released.</summary>
        [Precision(18, 3)]
        public decimal Balance { get; set; }

        // --- Relations ---

        /// <summary>1-1 owner account.</summary>
        [InverseProperty(nameof(Backend.Models.Entities.User.VendorProfile))]
        public User User { get; set; } = null!;

        /// <summary>M-1.</summary>
        [InverseProperty(nameof(Backend.Models.Entities.Category.VendorProfiles))]
        public Category Category { get; set; } = null!;

        /// <summary>1-M.</summary>
        [InverseProperty(nameof(Offer.VendorProfile))]
        public ICollection<Offer> Offers { get; set; } = new List<Offer>();

        /// <summary>1-M.</summary>
        [InverseProperty(nameof(Product.VendorProfile))]
        public ICollection<Product> Products { get; set; } = new List<Product>();

        /// <summary>1-M. Reviews received.</summary>
        [InverseProperty(nameof(Review.VendorProfile))]
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
