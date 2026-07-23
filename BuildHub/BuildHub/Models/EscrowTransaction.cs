using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

[Index(nameof(milestoneId), IsUnique = true)]
public class EscrowTransaction
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int escrowTransactionId { get; set; }

    [Required]
    public int contractId { get; set; }
    [ForeignKey("Contract")]
    public Contract Contract { get; set; }

    public int? milestoneId { get; set; }
    [ForeignKey("Milestone")]
    public Milestone Milestone { get; set; }

    [Required]
    [Column(TypeName = "decimal(12,2)")]
    public decimal amount { get; set; }

    [Required]
    [MaxLength(20)]
    public string status { get; set; } = "Held";
}