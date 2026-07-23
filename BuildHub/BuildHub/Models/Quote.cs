namespace BuildHub.Models;

public class Quote
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int quoteId { get; set; }  // system generated
    
    public int quoteRequestId { get; set; }   // foreign key
    [ForeignKey("QuoteRequest")]
    public QuoteRequest QuoteRequest { get; set; }
}