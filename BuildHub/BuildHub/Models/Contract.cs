using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

public class Contract
{
    [Required]
    [Key]
    public int contractId {  get; set; }//System generated



    [Required]
    [ForeignKey("Quote")]
    public int quoteId { get; set; }  // user input 
    public Quote quote { get; set; }



    [Required]
    public decimal totalAmount { get; set; }



    [Required]
    public string paymentType { get; set; }//User input

    [Required]
    public string status { get; set; }//

    [Required]
    public DateTime signedAt { get; set; }

    
}