namespace BuildHub.Models;

public class Review
{
    public int ReviewId { get; set; } //system generated
    public int ReviewerId { get; set; } //user input 
    public int ProductId { get; set; } //user input 
    public int VendorProfileId { get; set; } //user input 
    public int ContractId { get; set; } //user input 
    public int Rating { get; set; } //from list
    public string Comment  { get; set; } //user input 
    public DateTime ReviewDate { get; set; } = DateTime.UtcNow; //default value
}