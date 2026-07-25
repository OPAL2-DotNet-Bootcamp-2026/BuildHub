namespace BuildHub.Models;

public class Review
{
    public int ReviewId { get; set; } //system generated
    public int Rating { get; set; } //from list
    public string Comment  { get; set; } //user input 
    public DateTime ReviewDate { get; set; } = DateTime.UtcNow; //default value
}