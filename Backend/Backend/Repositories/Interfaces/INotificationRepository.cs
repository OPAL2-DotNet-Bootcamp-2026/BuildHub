using Backend.Models;
using Backend.Models.Entities;

namespace Backend.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetAllAsync();

        /// <summary>Returns null when no notification has this id.</summary>
        Task<Notification?> GetByIdAsync(int id);

        /// <summary>Every alert raised for one account, newest first.</summary>
        Task<IEnumerable<Notification>> GetByUserIdAsync(int userId);

        Task<Notification> CreateAsync(Notification notification);

        /// <summary>
        /// Marks the notification read or unread - the only thing about an alert that
        /// can change after it is sent. Returns null when the id does not exist.
        /// </summary>
        Task<Notification?> UpdateAsync(int id, Notification input);

        /// <summary>False when the id does not exist.</summary>
        Task<bool> DeleteAsync(int id);
    }
}
