namespace BuildHub.Models;

public class Review
{
    public int ReviewId { get; set; }
    public int Rating { get; set; }
    public string Comment  { get; set; }
    public DateTime ReviewDate { get; set; }
}