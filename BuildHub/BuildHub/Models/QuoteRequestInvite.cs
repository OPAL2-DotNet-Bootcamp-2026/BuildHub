using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

public class QuoteRequestInvite
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int InviteId { get; set; }  // system generated

    [ForeignKey(nameof(QuoteRequest))]
    public int QuoteRequestId { get; set; }   // foreign key
    public QuoteRequest QuoteRequest { get; set; }

    [ForeignKey(nameof(VendorProfile))]
    public int VendorProfileId { get; set; }  // foreign key
    public VendorProfile VendorProfile { get; set; }

    [Required]
    [MaxLength(20)]
    public string InviteStatus { get; set; } = "Sent";  // default value
}
