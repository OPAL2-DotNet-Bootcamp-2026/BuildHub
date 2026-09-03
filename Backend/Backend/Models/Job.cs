using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models
{
    /// <summary>
    /// A homeowner's request for work. The central object.
    /// </summary>
    public class Job
    {
        [Key]
        public int JobId { get; set; }

        [ForeignKey(nameof(Homeowner))]
        public int HomeownerId { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(4000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [MaxLength(80)]
        public string City { get; set; } = string.Empty;

        [Precision(18, 3)]
        public decimal? Budget { get; set; }

        public DateTime? Deadline { get; set; }

        public JobStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        // --- Relations ---

        /// <summary>M-1. The Homeowner-role user who posted this job.</summary>
        [InverseProperty(nameof(User.Jobs))]
        public User Homeowner { get; set; } = null!;

        /// <summary>M-1.</summary>
        [InverseProperty(nameof(Models.Category.Jobs))]
        public Category Category { get; set; } = null!;

        /// <summary>
        /// 1-M. Offers can only be submitted while <see cref="Status"/> is
        /// <see cref="JobStatus.Open"/>.
        /// </summary>
        [InverseProperty(nameof(Offer.Job))]
        public ICollection<Offer> Offers { get; set; } = new List<Offer>();

        // No Agreement navigation here on purpose. Job -> Agreement is derived:
        // Job -> Offer (the one Accepted) -> Agreement. A jobId on Agreement would
        // duplicate a fact already stored and the two could drift apart.
    }
}
