using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

public class Review
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ReviewId { get; set; } //system generated
    [Required]
    [ForeignKey("Reviewer")]
    public int ReviewerId { get; set; } //user input 
    public User Reviewer { get; set; }
    [ForeignKey("Product")]
    public int? ProductId { get; set; } //user input 
    public Product Product { get; set; }
    [ForeignKey("VendorProfile")]
    public int? VendorProfileId { get; set; } //user input 
    public VendorProfile VendorProfile { get; set; }
    [ForeignKey("Contract")]
    public int? ContractId { get; set; } //user input 
    public Contract Contract { get; set; }
    [Required]
    [Range(1, 5)]
    public int Rating { get; set; } //from list
    public string? Comment  { get; set; } //user input 
    [Required]
    public DateTime ReviewDate { get; set; } = DateTime.UtcNow; //default value
}