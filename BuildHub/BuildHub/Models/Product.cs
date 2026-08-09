using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProductId { get; set; }
    [Required]
    [ForeignKey(nameof(VendorProfile))]
    public int VendorProfileId { get; set; }
    public VendorProfile VendorProfile { get; set; }
    [Required]
    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    [Required]
    [MaxLength(150)]
    public string Name { get; set; }
    [Required]
    [MaxLength(20)]
    public string Unit { get; set; }
    [Required]
    [Column(TypeName = "decimal(12,2)")]
    public decimal CurrentPrice { get; set; }
    [Required]
    public bool IsAvailable { get; set; }

}
