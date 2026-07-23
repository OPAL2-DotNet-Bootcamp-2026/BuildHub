using BuildHub.enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

public class Project
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProjectId { get; set; } //system genterated 
    [Required]
    [ForeignKey("User")]
    public int ClientId { get; set; } //user input 
    public User User { get; set; }
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } //user input 
    public string? Description { get; set; } //user input 
    [Required]
    [MaxLength(100)]
    public string City { get; set; } //user input 
    [Required]
    [Range(0, double.MaxValue)]
    [Column(TypeName = "decimal(12,2)")]
    public decimal Budget { get; set; } //user input 
    [Required]
    [MaxLength(20)]
    public ProjectStatus Status { get; set; } = ProjectStatus.Draft; //from list 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; //default value 
}