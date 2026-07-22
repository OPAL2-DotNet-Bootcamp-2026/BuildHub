namespace BuildHub.Models;

public class QuoteRequestInvite
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int inviteId { get; set; }  // system generated
    
}