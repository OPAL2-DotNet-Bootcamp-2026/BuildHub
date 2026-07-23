using Microsoft.EntityFrameworkCore;

namespace BuildHub.Models;

public class Project
{
    public int ProjectId { get; set; }
    public int ClientId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string City { get; set; }
    public decimal Budget { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}