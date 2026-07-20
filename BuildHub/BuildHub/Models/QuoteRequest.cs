namespace BuildHub.Models;

public class QuoteRequest
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int qutoeRequestId { get; set; }  // system generated
    
    public int projectId { get; set; }     // foreign key
    [ForeignKey("Project")]
    public Project Project { get; set; }
    public int categoryId { get; set; }     // foreign key
    [ForeignKey("Category")]
    public Category Category { get; set; }
    [Required]
    public string description { get; set; }     // user input
    public DateTime? deadline { get; set; }     // user input
    [Required]
    [MaxLength(10)]
    public string visibilityType { get; set; }     // user input
}