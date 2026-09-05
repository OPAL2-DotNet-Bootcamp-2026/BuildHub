
namespace Backend.Models.Dtos
{
    /// <summary>
    /// What the API gives back for a user. PasswordHash is deliberately absent -
    /// the entity is never serialised directly, so the hash cannot leak.
    /// </summary>
    public class UserResponse
    {
        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public UserRole Role { get; set; }

        public string City { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
