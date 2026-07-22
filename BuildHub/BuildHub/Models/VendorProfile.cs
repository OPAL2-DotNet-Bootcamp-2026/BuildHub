using BuildHub.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

public class VendorProfile
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int VendorProfileID { get; set; } //system generated
    [Required]
    [ForeignKey("user")]
    public int UserId { get; set; } //user input
    public User User { get; set; } //navigation property 
    [Required]
    [MaxLength(150)]
    public string CompanyName { get; set; } //user input
    [Required]
    [MaxLength(20)]
    public VendorType VendorType { get; set; } //from list
    [Required]
    [MaxLength(100)]
    public string City { get; set; } //user input
    [Required]
    public bool IsVerfied { get; set; } = false; //defualt values
    public double? AverageRating { get; set; } = 0; //defualt values
    [Required]
    [Range(0, double.MaxValue)]
    public double Balance { get; set; } = 0; //defualt values
}