using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

public class QuoteNegotiation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int QuoteNegotiationId { get; set; } //System generated

    [Required]
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }  //foreign key
    public User User { get; set; }


    [Required]
    [ForeignKey(nameof(Quote))]
    public int QuoteId { get; set; } //foreign key
    public Quote Quote { get; set; }


    [Required, Range(0, 10)]
    public decimal ProposedPrice { get; set; }//user input


    public string? ProposedDurationDays { get; set; }//user input

    [MaxLength(1000)]
    public string? Message { get; set; }//user input


    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now; // system generated
}
