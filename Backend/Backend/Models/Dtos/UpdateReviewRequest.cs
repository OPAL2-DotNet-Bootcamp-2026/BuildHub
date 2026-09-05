using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Dtos
{
    public class UpdateReviewRequest
    {
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(2000)]
        public string? Comment { get; set; }
    }
}
