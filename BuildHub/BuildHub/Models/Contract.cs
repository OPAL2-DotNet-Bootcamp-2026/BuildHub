using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;



[Index (nameof(quoteId),IsUnique = true )]
public class Contract
{
    [Required]
    [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int contractId {  get; set; }//System generated



    [Required]
    [ForeignKey("Quote")]
    public int quoteId { get; set; }  // user input 
    public Quote quote { get; set; }



    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Should be more than 0")]
    public decimal totalAmount { get; set; }//User input 



    [Required]
    [AllowedValues("One time ","PreMilestone")]
    public string paymentType { get; set; }//User input

    [Required]
    [AllowedValues("Active", "Completed", "Disputted", "Cancelled")]
    public string status { get; set; } = "Active";//Defult value 

    [Required]
    public DateTime signedAt { get; set; } = DateTime.UtcNow;// User input

    
}