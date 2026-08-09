using System.ComponentModel.DataAnnotations;

namespace BuildHub.DTOs
{

    // Input
    public class QuoteNegotiationInputDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int QuoteId { get; set; }

        // NOTE: mirrors the Range(0,10) still declared on the QuoteNegotiation
        // entity, so binding a DTO does not silently change what is accepted.
        // That cap is almost certainly a bug - real prices exceed 10.
        [Required, Range(0, 10)]
        public decimal ProposedPrice { get; set; }

        public string? ProposedDurationDays { get; set; }

        [MaxLength(1000)]
        public string? Message { get; set; }
    }



    // Resonse / Output
    public class QuoteNegotiationOutputDto
    {
        public int QuoteNegotiationId { get; set; }
        public int UserId { get; set; }
        public decimal ProposedPrice { get; set; }
        public string ProposedDurationDays { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
