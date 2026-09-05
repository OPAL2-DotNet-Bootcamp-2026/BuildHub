namespace Backend.Models.Dtos
{
    /// <summary>
    /// Marks an alert read or unread - the only thing that changes after it is sent.
    /// </summary>
    public class UpdateNotificationRequest
    {
        public bool IsRead { get; set; }
    }
}
