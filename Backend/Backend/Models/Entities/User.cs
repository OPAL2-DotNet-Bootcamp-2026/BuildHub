using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models.Entities
{
    /// <summary>
    /// The account. One table for everyone; <see cref="Role"/> distinguishes them.
    /// </summary>
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string PasswordHash { get; set; } = string.Empty;

        [Phone]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        public UserRole Role { get; set; }

        [Required]
        [MaxLength(80)]
        public string City { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        // --- Relations ---

        /// <summary>1-1. Only Vendor-role users have one.</summary>
        [InverseProperty(nameof(Backend.Models.Entities.VendorProfile.User))]
        public VendorProfile? VendorProfile { get; set; }

        /// <summary>1-M. Jobs posted by this user as a homeowner.</summary>
        [InverseProperty(nameof(Job.Homeowner))]
        public ICollection<Job> Jobs { get; set; } = new List<Job>();

        /// <summary>1-M. Reviews written by this user.</summary>
        [InverseProperty(nameof(Review.Reviewer))]
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        /// <summary>1-M.</summary>
        [InverseProperty(nameof(Notification.User))]
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
