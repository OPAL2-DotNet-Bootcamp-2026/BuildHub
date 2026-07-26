using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int productId { get; set; }
    [Required]
    [ForeignKey("vendorProfile")]
    public int vendorProfileId { get; set; }
    public  VendorProfile vendorProfile { get; set; }
    [Required]
    [ForeignKey("category")]
    public int categoryId { get; set; }
    public  Category category { get; set; }
    [Required]
    [MaxLength(150)]
    public string name { get; set; }
    [Required]
    [MaxLength(20)]
    public string unit { get; set; }
    [Required]
    [Column(TypeName = "decimal(12,2)")]
    public decimal currentPrice { get; set; }
    [Required]
    public bool isAvailable { get; set; }

}