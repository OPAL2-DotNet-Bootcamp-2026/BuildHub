
namespace Backend.Models.Dtos
{
    public class NotificationResponse
    {
        public int NotificationId { get; set; }

        public int UserId { get; set; }

        public string Message { get; set; } = string.Empty;

        public NotificationType Type { get; set; }

        /// <summary>Deep-link target; what it points at depends on <see cref="Type"/>.</summary>
        public int? RelatedId { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
