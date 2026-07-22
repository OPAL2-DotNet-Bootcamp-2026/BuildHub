using BuildHub.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

public class VendorProfile
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int VendorProfileID { get; set; }
    [Required]
    [ForeignKey("user")]
    public int UserId { get; set; }
    public User User { get; set; }
    [Required]
    [MaxLength(150)]
    public string CompanyName { get; set; }
    [Required]
    [MaxLength(20)]
    public VendorType VendorType { get; set; }
    [Required]
    [MaxLength(100)]
    public string City { get; set; }
    [Required]
    public bool IsVerfied { get; set; } = false;
    public double? AverageRating { get; set; } = 0;
    [Required]
    [Range(0, double.MaxValue)]
    public double Balance { get; set; } = 0;
}