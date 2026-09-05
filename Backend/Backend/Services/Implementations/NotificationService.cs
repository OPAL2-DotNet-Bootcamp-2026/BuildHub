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
        private readonly ICurrentUser _currentUser;

        public NotificationService(
            INotificationRepository notificationRepository,
            IUserRepository userRepository,
            ICurrentUser currentUser)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        /// <summary>
        /// Your own alerts. An administrator sees everyone's; nobody else does, so this
        /// list cannot be used to read another account's activity.
        /// </summary>
        public async Task<IEnumerable<NotificationResponse>> GetAllAsync()
        {
            var notifications = _currentUser.IsAdmin
                ? await _notificationRepository.GetAllAsync()
                : await _notificationRepository.GetByUserIdAsync(_currentUser.UserId);

            return notifications.Select(ToResponse);
        }

        public async Task<NotificationResponse?> GetByIdAsync(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            if (notification is null) return null;

            EnsureAddressedToCaller(notification);
            return ToResponse(notification);
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
            var existing = await _notificationRepository.GetByIdAsync(id);
            if (existing is null) return null;

            EnsureAddressedToCaller(existing);

            var updated = await _notificationRepository.UpdateAsync(id, new Notification
            {
                IsRead = request.IsRead
            });

            return updated is null ? null : ToResponse(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _notificationRepository.GetByIdAsync(id);
            if (existing is null) return false;

            EnsureAddressedToCaller(existing);

            return await _notificationRepository.DeleteAsync(id);
        }

        private void EnsureAddressedToCaller(Notification notification)
        {
            if (!_currentUser.IsAdmin && notification.UserId != _currentUser.UserId)
            {
                throw new ForbiddenException(
                    $"Notification {notification.NotificationId} was raised for another account.");
            }
        }

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
