using Backend.Data;
using Backend.Models;
using Backend.Models.Entities;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories.Implementations
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly BuildHubDbContext _context;

        public NotificationRepository(BuildHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetAllAsync() =>
            await _context.Notifications.AsNoTracking().ToListAsync();

        public async Task<Notification?> GetByIdAsync(int id) =>
            await _context.Notifications.AsNoTracking()
                .FirstOrDefaultAsync(n => n.NotificationId == id);

        public async Task<Notification> CreateAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<Notification?> UpdateAsync(int id, Notification input)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification is null) return null;

            // Read state is the only thing that changes after an alert is sent.
            notification.IsRead = input.IsRead;

            // Deliberately not updated here:
            //   UserId, Message, Type, RelatedId - a notification is a record of
            //     something that already happened; rewriting it would rewrite history
            //     and could point the deep-link at an unrelated record
            //   CreatedAt - a historical fact

            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification is null) return false;

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
