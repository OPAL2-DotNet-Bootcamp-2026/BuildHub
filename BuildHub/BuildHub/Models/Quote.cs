using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

public class Quote
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int quoteId { get; set; }  // system generated

    public int quoteRequestId { get; set; }   // foreign key
    [ForeignKey("QuoteRequest")]
    public QuoteRequest QuoteRequest { get; set; }

    public int vendorProfileId { get; set; }  // foreign key
    [ForeignKey("VendorProfile")]
    public VendorProfile VendorProfile { get; set; }

    public int? parentQuoteId { get; set; }   // foreign key
    [ForeignKey("ParentQuote")]
    public Quote ParentQuote { get; set; }

    [Required]
    [Column(TypeName = "decimal(12,2)")]
    [Range(0, double.MaxValue)]
    public decimal price { get; set; }   // user input

    [Required]
    public int durationDays { get; set; }   // user input

    [Required]
    [MaxLength(20)]
    public string status { get; set; } = "Pending";   // default value

    [Required]
    public DateTime submittedAt { get; set; } = DateTime.UtcNow;   // default value
}