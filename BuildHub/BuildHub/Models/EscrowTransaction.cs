using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

public class EscrowTransaction
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int escrowTransactionId { get; set; }
}