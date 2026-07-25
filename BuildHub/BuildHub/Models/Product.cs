using System.ComponentModel.DataAnnotations;

namespace BuildHub.Models;

public class Product
{
    [Key]
    public int productId { get; set; }
    [Required]
    public int vendorProfileId { get; set; }
    [Required]
    public int categoryId { get; set; }
    [Required]
    public string name { get; set; }
    [Required]
    public string unit { get; set; }
    [Required]
    public int currentPrice { get; set; }
    [Required]
    public bool isAvailable { get; set; }

}