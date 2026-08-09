using BuildHub.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

[Index(nameof(UserId), IsUnique = true)]
public class VendorProfile
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int VendorProfileId { get; set; } //system generated
    [Required]
    [ForeignKey(nameof(User))]
    public int UserId { get; set; } //user input
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
    public bool IsVerified { get; set; } = false; //defualt values
    public decimal? AverageRating { get; set; } = 0; // default value
    [Required]
    [Range(typeof(decimal), "0", "79228162514264337593543950335")] // decimal.MaxValue as a string literal
    public decimal Balance { get; set; } = 0; // default value


    //navigation property
    public User User { get; set; }
    public List<Product> Products { get; set; }
    public List<Quote> Quotes { get; set; }
    public List<QuoteRequestInvite> QuoteRequestInvites { get; set; }
    public List<Review> Reviews { get; set; }
}
