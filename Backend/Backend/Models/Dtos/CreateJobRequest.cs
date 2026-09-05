using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Dtos
{
    /// <summary>
    /// Posts a job. Status is absent - a new job is always Open, decided by the
    /// service rather than the caller.
    /// </summary>
    public class CreateJobRequest
    {
        [Range(1, int.MaxValue)]
        public int HomeownerId { get; set; }

        [Range(1, int.MaxValue)]
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

        [Range(0, 9999999999999.999)]
        public decimal? Budget { get; set; }

        public DateTime? Deadline { get; set; }
    }
}
