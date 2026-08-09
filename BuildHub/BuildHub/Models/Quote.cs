using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

public class Quote
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int QuoteId { get; set; }                 // system generated


    [Required]
    [ForeignKey(nameof(QuoteRequest))]
    public int QuoteRequestId { get; set; }
    public QuoteRequest QuoteRequest { get; set; }


    [Required]
    [ForeignKey(nameof(VendorProfile))]
    public int VendorProfileId { get; set; }
    public VendorProfile VendorProfile { get; set; }


    [Required]
    [Column(TypeName = "decimal(12,2)")]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }


    [Required]
    public int DurationDays { get; set; }


    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Pending";


    [Required]
    public DateTime SubmittedAt { get; set; }
}
