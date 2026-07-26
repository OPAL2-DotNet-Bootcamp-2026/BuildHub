using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

public class QuoteRequestInvite
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int inviteId { get; set; }  // system generated

    [ForeignKey("QuoteRequest")]
    public int quoteRequestId { get; set; }   // foreign key
    public QuoteRequest QuoteRequest { get; set; }

    [ForeignKey("VendorProfile")]
    public int vendorProfileId { get; set; }  // foreign key
    public VendorProfile VendorProfile { get; set; }

    [Required]
    [MaxLength(20)]
    public string inviteStatus { get; set; } = "Sent";  // default value
}