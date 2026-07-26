using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

public class Notification
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int notificationId {  get; set; }
    [Required]
    [ForeignKey(nameof(User))]
    public int userId { get; set; }
    public User User { get; set; }
    [Required]
    [MaxLength(150)]
    public string title { get; set; }
    [Required]
    [MaxLength(30)]
    public string type { get; set; }
    [Required]
    public bool isRead { get; set; }
    [Required]
    public DateTime createdAt { get; set; }
}