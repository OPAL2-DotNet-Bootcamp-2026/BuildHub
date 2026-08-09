using BuildHub.DTOs;
using BuildHub.Models;
using BuildHub.Repos;

namespace BuildHub.Services
{
    public class NotificationService
    {
        private NotificationRepo notificationRepo;

        public NotificationService(NotificationRepo notificationRepo)
        {
            this.notificationRepo = notificationRepo;
        }


        public void CreateNotification(int userId, string title, string type)
        {
            Notification notification = new Notification();
            notification.UserId = userId;
            notification.Title = title;
            notification.Type = type;
            notification.IsRead = false;
            notification.CreatedAt = DateTime.Now;

            notificationRepo.Add(notification);
        }

        // for the notifications endpoint
        public List<NotificationOutputDto> GetByUserId(int userId)
        {
            List<Notification> notifications = notificationRepo.GetByUserId(userId);
            List<NotificationOutputDto> result = new List<NotificationOutputDto>();

            foreach (Notification n in notifications)
            {
                result.Add(MapToOutput(n));
            }

            return result;
        }

        // Converter function
        private NotificationOutputDto MapToOutput(Notification n)
        {
            // Create an empty object
            NotificationOutputDto dto = new NotificationOutputDto();
            dto.NotificationId = n.NotificationId;
            dto.UserId = n.UserId;
            dto.Title = n.Title;
            dto.Type = n.Type;
            dto.IsRead = n.IsRead;
            dto.CreatedAt = n.CreatedAt;
            return dto;
        }
    }
}
