using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Dtos
{
    /// <summary>
    /// Raises an alert. IsRead is absent - a new notification is always unread.
    /// </summary>
    public class CreateNotificationRequest
    {
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Message { get; set; } = string.Empty;

        [EnumDataType(typeof(NotificationType), ErrorMessage = "Type is not a known notification type.")]
        public NotificationType Type { get; set; }

        public int? RelatedId { get; set; }
    }
}
