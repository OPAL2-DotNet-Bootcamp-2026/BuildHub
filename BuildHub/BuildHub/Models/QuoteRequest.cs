 using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

public class QuoteRequest
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int qutoeRequestId { get; set; }  // system generated

    [ForeignKey("Project")]
    public int projectId { get; set; }     // foreign key
    public Project Project { get; set; }

    [ForeignKey("Category")]
    public int categoryId { get; set; }     // foreign key
    public Category Category { get; set; }

    [Required]
    public string description { get; set; }     // user input

    public DateTime? deadline { get; set; }     // user input

    [Required]
    [MaxLength(10)]
    public string visibilityType { get; set; }     // user input

    [Required]
    [MaxLength(20)]
    public string status { get; set; } = "Open";   // default value
}