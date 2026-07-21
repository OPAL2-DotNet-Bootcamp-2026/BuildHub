using System.ComponentModel.DataAnnotations;

namespace BuildHub.DTOs
{

    // Input
    public class QuoteNegotiationInputDTO
    {
        [Required, MaxLength(255)]
        public string message { get; set; }

    }



    // Resonse / Output 
    public class QuoteNegotiationOutputDTO
    {
        public int quoteNegotiationId { get; set; }
        public int userId { get; set; }
        public decimal proposedPrice { get; set; }
        public string proposedDurationDays { get; set; }
        public DateTime createIn { get; set; }

    }







}
 