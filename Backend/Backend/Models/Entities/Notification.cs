using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Entities
{
    /// <summary>
    /// In-app alert.
    /// </summary>
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Message { get; set; } = string.Empty;

        public NotificationType Type { get; set; }

        /// <summary>
        /// Deep-link target - the id of the job/offer/agreement/review this concerns.
        /// Deliberately not a foreign key: what it points at depends on <see cref="Type"/>.
        /// </summary>
        public int? RelatedId { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }

        // --- Relations ---

        /// <summary>M-1. The recipient.</summary>
        [InverseProperty(nameof(Backend.Models.Entities.User.Notifications))]
        public User User { get; set; } = null!;
    }
}
