using BuildHub.DTOs;
using BuildHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildHub.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    // [Authorize]
    public class NotificationController : ControllerBase
    {
        private NotificationService notificationService;

        public NotificationController(NotificationService _notificationService) //dependency injection
        {
            notificationService = _notificationService;
        }

        // GET http://localhost:5153/api/notifications/3
        [HttpGet("{userId}")]
        public IActionResult GetMyNotifications([FromRoute] int userId)
        {
            List<NotificationOutputDTO> result = notificationService.GetByUserId(userId);

            if (result.Count > 0)
                return Ok(result);

            return NoContent(); //204 no data
        }
    }
}