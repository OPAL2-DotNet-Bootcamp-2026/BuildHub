using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

[Index(nameof(MilestoneId), IsUnique = true)]
public class EscrowTransaction
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EscrowTransactionId { get; set; }

    [Required]
    [ForeignKey(nameof(Contract))]
    public int ContractId { get; set; }
    public Contract Contract { get; set; }

    [ForeignKey(nameof(Milestone))]
    public int? MilestoneId { get; set; }
    public Milestone Milestone { get; set; }

    [Required]
    [Column(TypeName = "decimal(12,2)")]
    public decimal Amount { get; set; }

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Held";

    public DateTime? HeldAt { get; set; }

    public DateTime? ReleasedAt { get; set; }

}
