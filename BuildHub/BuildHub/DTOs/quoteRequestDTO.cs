using System.ComponentModel.DataAnnotations;

namespace BuildHub.DTOs
{
    public class QuoteRequestInputDTOs
    {

        [Required(ErrorMessage = "Value should not be null.")]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Value should not be null.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Value should not be null.")]
        public string Description { get; set; }

        public DateTime? Deadline { get; set; }

        [Required(ErrorMessage = "Value should not be null.")]
        [MaxLength(10)]
        public string VisibilityType { get; set; }
    }
    public class QuoteRequestOutputDTOs
        public int QuoteRequestId { get; set; }
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        public string VisibilityType { get; set; }
}
