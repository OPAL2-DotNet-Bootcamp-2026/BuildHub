namespace BuildHub.Models;

public class Quote
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int quoteId { get; set; }  // system generated
    
}