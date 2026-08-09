using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

public class QuoteRequest
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int QuoteRequestId { get; set; }  // system generated

    [ForeignKey(nameof(Project))]
    public int ProjectId { get; set; }     // foreign key
    public Project Project { get; set; }

    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }     // foreign key
    public Category Category { get; set; }

    [Required]
    public string Description { get; set; }     // user input

    public DateTime? Deadline { get; set; }     // user input

    [Required]
    [MaxLength(10)]
    public string VisibilityType { get; set; }     // user input

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Open";   // default value
}
