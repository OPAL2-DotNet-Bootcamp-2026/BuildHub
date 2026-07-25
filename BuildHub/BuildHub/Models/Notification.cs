using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

public class Notification
{
    [Key]
    public int notificationId {  get; set; }
    [Required]
    [ForeignKey(nameof(User))]
    public int userId { get; set; }
    [Required]
    public string title { get; set; }
    [Required]
    public string type { get; set; }
    [Required]
    public bool isRead { get; set; }
    [Required]
    public DateTime createdAt { get; set; }
}