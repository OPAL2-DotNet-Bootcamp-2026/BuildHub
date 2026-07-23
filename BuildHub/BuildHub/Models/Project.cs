using Microsoft.EntityFrameworkCore;

namespace BuildHub.Models;

public class Project
{
    public int ProjectId { get; set; } //system genterated 
    public int ClientId { get; set; } //user input 
    public string Title { get; set; } //user input 
    public string Description { get; set; } //user input 
    public string City { get; set; } //user input 
    public decimal Budget { get; set; } //user input 
    public string Status { get; set; } //from list 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; //default value 
}