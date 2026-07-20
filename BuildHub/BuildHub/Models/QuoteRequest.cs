namespace BuildHub.Models;

public class QuoteRequest
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int qutoeRequestId { get; set; }  // system generated
    
    public int projectId { get; set; }     // foreign key
    [ForeignKey("Project")]
    public Project Project { get; set; }
}