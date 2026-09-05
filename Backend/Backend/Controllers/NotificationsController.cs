using Backend.Models;
using Backend.Models.Dtos;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>Lists every notification.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<NotificationResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<NotificationResponse>>> GetAll()
        {
            return Ok(await _notificationService.GetAllAsync());
        }

        /// <summary>Gets one notification by id.</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(NotificationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<NotificationResponse>> GetById(int id)
        {
            var notification = await _notificationService.GetByIdAsync(id);
            return notification is null ? NotFound() : Ok(notification);
        }

        /// <summary>Raises an unread alert for a user.</summary>
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost]
        [ProducesResponseType(typeof(NotificationResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<NotificationResponse>> Create(CreateNotificationRequest request)
        {
            var created = await _notificationService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.NotificationId }, created);
        }

        /// <summary>Marks it read or unread - the only thing that changes after sending.</summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(NotificationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<NotificationResponse>> Update(
            int id, UpdateNotificationRequest request)
        {
            var updated = await _notificationService.UpdateAsync(id, request);
            return updated is null ? NotFound() : Ok(updated);
        }

        /// <summary>Deletes a notification.</summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            return await _notificationService.DeleteAsync(id) ? NoContent() : NotFound();
        }
    }
}
