using Backend.Exceptions;
using Backend.Models.Dtos;
using Backend.Models;
using Backend.Models.Entities;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;

namespace Backend.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;

        public NotificationService(
            INotificationRepository notificationRepository,
            IUserRepository userRepository)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<NotificationResponse>> GetAllAsync()
        {
            var notifications = await _notificationRepository.GetAllAsync();
            return notifications.Select(ToResponse);
        }

        public async Task<NotificationResponse?> GetByIdAsync(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            return notification is null ? null : ToResponse(notification);
        }

        public async Task<NotificationResponse> CreateAsync(CreateNotificationRequest request)
        {
            // Checked so a bad recipient id is a 404, not a foreign-key 500.
            if (await _userRepository.GetByIdAsync(request.UserId) is null)
            {
                throw new NotFoundException($"No user with id {request.UserId}.");
            }

            var created = await _notificationRepository.CreateAsync(new Notification
            {
                UserId = request.UserId,
                Message = request.Message.Trim(),
                Type = request.Type,
                RelatedId = request.RelatedId,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            });

            return ToResponse(created);
        }

        public async Task<NotificationResponse?> UpdateAsync(int id, UpdateNotificationRequest request)
        {
            var updated = await _notificationRepository.UpdateAsync(id, new Notification
            {
                IsRead = request.IsRead
            });

            return updated is null ? null : ToResponse(updated);
        }

        public async Task<bool> DeleteAsync(int id) =>
            await _notificationRepository.DeleteAsync(id);

        private static NotificationResponse ToResponse(Notification notification) => new()
        {
            NotificationId = notification.NotificationId,
            UserId = notification.UserId,
            Message = notification.Message,
            Type = notification.Type,
            RelatedId = notification.RelatedId,
            IsRead = notification.IsRead,
            CreatedAt = notification.CreatedAt
        };
    }
}
