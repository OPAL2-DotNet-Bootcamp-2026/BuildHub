using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

public class QuoteNegotiation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int quoteNegotiationId { get; set; } //System generated

    [Required]
    [ForeignKey("senderId")]
    public int senderId { get; set; }  //foreign key



    [Required]
    [ForeignKey(nameof(Quote))]
    public int quoteId { get; set; } //foreign key
    public Quote Quote { get; set; }


    [Required,Range(0,10) ]
    public decimal proposedPrice { get; set; }//user input


    public int? proposedDurationDays { get; set; }//user input

    [MaxLength(1000)]
    public string? message { get; set; }//user input


    [Required]
    public DateTime create { get; set; }  = DateTime.Now;




     

    
}