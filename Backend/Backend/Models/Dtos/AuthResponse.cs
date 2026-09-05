using Backend.Models;

namespace Backend.Models.Dtos
{
    /// <summary>
    /// A successful sign-in: the bearer token plus just enough about the account for
    /// a client to render itself without decoding the token.
    /// </summary>
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public UserRole Role { get; set; }
    }
}
