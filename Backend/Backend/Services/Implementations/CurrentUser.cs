using System.Security.Claims;
using Backend.Exceptions;
using Backend.Models;
using Backend.Services.Interfaces;

namespace Backend.Services.Implementations
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? Principal => _httpContextAccessor.HttpContext?.User;

        public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated == true;

        public int UserId
        {
            get
            {
                var raw = Principal?.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!int.TryParse(raw, out var userId))
                {
                    throw new UnauthorizedException("This request is not signed in.");
                }

                return userId;
            }
        }

        public UserRole? Role =>
            Enum.TryParse<UserRole>(Principal?.FindFirstValue(ClaimTypes.Role), out var role)
                ? role
                : null;

        public bool IsAdmin => Role == UserRole.Admin;
    }
}
