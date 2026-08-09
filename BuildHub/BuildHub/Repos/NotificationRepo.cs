using BuildHub.Models;

namespace BuildHub.Repos
{
    public class NotificationRepo
    {
        // This is a projectContext context = new ProjectContext();
        private ProjectContext context;

        public NotificationRepo(ProjectContext context)
        {
            this.context = context;
        }

        public List<Notification> GetAll()
        {
            return context.Notifications.ToList();
        }

        public Notification GetById(int notificationId)
        {
            return context.Notifications.FirstOrDefault(c => c.NotificationId == notificationId);
        }

        public List<Notification> GetByUserId(int userId)
        {
            return context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToList();
        }

        public void Add(Notification notification)
        {
            context.Notifications.Add(notification);
            context.SaveChanges();
        }

        public void Update()
        {
            context.SaveChanges();
        }

        public void Delete(Notification notification)
        {
            context.Notifications.Remove(notification);
            context.SaveChanges();
        }
    }
}
