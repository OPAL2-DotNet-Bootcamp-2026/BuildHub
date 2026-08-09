using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

public class Milestone
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MilestoneId { get; set; } //System genreated


    [Required]
    [ForeignKey(nameof(Contract))]
    public int ContractId { get; set; } //User input
    public Contract Contract { get; set; }


    [Required]
    [MaxLength(150)]
    public string Title { get; set; } //User input


    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Should be more than 0 ")]
    public decimal Amount { get; set; } // User input


    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Should be more than 0")]
    public int OrderIndex { get; set; } // user input


    [Required]
    [MaxLength(20)]
    [AllowedValues("Pending", "InProgress", "Submitted For Review", "Approved", "Rejected")]
    public string Status { get; set; } = "Pending";//Defualt value

    public DateTime EndDate { get; set; } // I added for contract Service



    public DateTime? DueDate { get; set; } // user input


}
