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
    }
}
