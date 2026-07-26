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
    [ForeignKey("Contract")]
    public int contractId { get; set; }
    public Contract Contract { get; set; }


    [ForeignKey("Milestone")]
    public int? milestoneId { get; set; }
    public Milestone Milestone { get; set; }

    [Required]
    [Column(TypeName = "decimal(12,2)")]
    public decimal amount { get; set; }

    [Required]
    [MaxLength(20)]
    public string status { get; set; } = "Held";

    public DateTime? heldAt { get; set; }

    public DateTime? releasedAt { get; set; }

}