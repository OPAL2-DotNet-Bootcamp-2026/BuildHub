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
    }
}
