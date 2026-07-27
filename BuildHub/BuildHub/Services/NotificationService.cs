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
    }
}
