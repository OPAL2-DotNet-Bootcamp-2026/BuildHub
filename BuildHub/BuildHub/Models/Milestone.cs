using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

public class Milestone
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int milestoneId { get; set; } //System genreated 


    [Required]
    [ForeignKey("Contract")]
    public int contractId { get; set; } //User input


    [Required]
    [MaxLength(150)]
    public string title { get; set; } //User input 


    [Required]
    [Range(0.01, double.MaxValue,ErrorMessage = "Should be more than 0 ")]
    public decimal amount { get; set; } // User input


    [Required]
    [Range(0,int.MaxValue,ErrorMessage ="Should be more than 0")]
    public int orderIndex { get; set; } // user input 


    [Required]
    public string status { get; set; }//Defualt value 



    public DateTime? dueDate { get; set; } // user input

    
}