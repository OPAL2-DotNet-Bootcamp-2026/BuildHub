using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;



[Index(nameof(QuoteId), IsUnique = true)]
public class Contract
{
    [Required]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ContractId { get; set; }//System generated



    [Required]
    [ForeignKey(nameof(Quote))]
    public int QuoteId { get; set; }  // user input
    public Quote Quote { get; set; }



    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Should be more than 0")]
    public decimal TotalAmount { get; set; }//User input



    [Required]
    [AllowedValues("One time ", "PreMilestone")]
    public string PaymentType { get; set; }//User input

    [Required]
    [AllowedValues("Active", "Completed", "Disputted", "Cancelled")]
    public string Status { get; set; } = "Active";//Defult value

    [Required]
    public DateTime SignedAt { get; set; } = DateTime.UtcNow;// User input

    public List<Milestone> Milestones { get; set; }
    public List<EscrowTransaction> EscrowTransactions { get; set; }


}
