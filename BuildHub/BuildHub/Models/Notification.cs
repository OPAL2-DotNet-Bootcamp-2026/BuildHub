namespace BuildHub.Models;

public class Notification
{
    public int notificationId {  get; set; }
    public int userId { get; set; }
    public string title { get; set; }
    public string type { get; set; }
    public bool isRead { get; set; }
    public DateTime createdAt { get; set; }
}