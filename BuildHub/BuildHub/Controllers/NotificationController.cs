using BuildHub.DTOs;
using BuildHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildHub.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {
        private NotificationService notificationService;

        public NotificationController(NotificationService _notificationService) //dependency injection
        {
            notificationService = _notificationService;
        }
    }
}
