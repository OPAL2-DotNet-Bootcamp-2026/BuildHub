using System.ComponentModel.DataAnnotations;

namespace BuildHub.DTOs
{
    public class QuoteRequestInputDTOs
        [Required(ErrorMessage = "Value should not be null.")]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Value should not be null.")]
        public int CategoryId { get; set; }
}
