using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

public class QuoteNegotiation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int quoteNegotiation { get; set; }

    [Required]
    [ForeignKey("senderId")]
    public int senderId { get; set; }  //ننتظر صاحب المودل هذه يسويها علشان نسوي برايمري كي مالها الصحيحة 

    [Required,Range(0,10) ]
    public decimal proposedPrice { get; set; }


    public int? proposedDurationDays { get; set; }

    [MaxLength(1000)]
    public string? message { get; set; }


    [Required]
    public DateTime create { get; set; }  = DateTime.Now;




     

    
}