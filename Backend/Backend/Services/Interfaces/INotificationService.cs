using Backend.Exceptions;
using Backend.Models.Dtos;

namespace Backend.Services.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationResponse>> GetAllAsync();

        /// <summary>Null when no notification has this id.</summary>
        Task<NotificationResponse?> GetByIdAsync(int id);

        /// <summary>
        /// Raises an unread alert.
        /// Throws <see cref="NotFoundException"/> when the recipient does not exist.
        /// </summary>
        Task<NotificationResponse> CreateAsync(CreateNotificationRequest request);

        /// <summary>Marks it read or unread. Null when no notification has this id.</summary>
        Task<NotificationResponse?> UpdateAsync(int id, UpdateNotificationRequest request);

        /// <summary>False when no notification has this id.</summary>
        Task<bool> DeleteAsync(int id);
    }
}
