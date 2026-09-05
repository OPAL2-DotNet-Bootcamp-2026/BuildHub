
namespace Backend.Models.Dtos
{
    public class JobResponse
    {
        public int JobId { get; set; }

        public int HomeownerId { get; set; }

        public int CategoryId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public decimal? Budget { get; set; }

        public DateTime? Deadline { get; set; }

        public JobStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
