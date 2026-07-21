using System.ComponentModel.DataAnnotations;

namespace BuildHub.DTOs
{
    public class QuoteRequestInputDTOs
        [Required(ErrorMessage = "Value should not be null.")]
        public int ProjectId { get; set; }

}
