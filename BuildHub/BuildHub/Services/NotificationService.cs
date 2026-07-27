using BuildHub.DTOs;
using BuildHub.Models;
using BuildHub.Repos;

﻿namespace BuildHub.Services
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
            notification.userId = userId;
            notification.title = title;
            notification.type = type;
            notification.isRead = false;
            notification.createdAt = DateTime.Now;

            notificationRepo.Add(notification);
        }

        // for the notifications endpoint
        public List<NotificationOutputDTO> GetByUserId(int userId)
        {
            List<Notification> notifications = notificationRepo.GetByUserId(userId);
            List<NotificationOutputDTO> result = new List<NotificationOutputDTO>();

            foreach (Notification n in notifications)
            {
                result.Add(MapToOutput(n));
            }

            return result;
        }
    }
}
