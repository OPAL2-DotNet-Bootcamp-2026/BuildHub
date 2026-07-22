namespace BuildHub.Models;

public class QuoteRequestInvite
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int inviteId { get; set; }  // system generated
    
    public int quoteRequestId { get; set; }   // foreign key
    [ForeignKey("QuoteRequest")]
    public QuoteRequest QuoteRequest { get; set; }
    public int vendorProfileId { get; set; }  // foreign key
}