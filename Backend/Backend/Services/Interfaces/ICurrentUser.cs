using Backend.Exceptions;
using Backend.Models;

namespace Backend.Services.Interfaces
{
    /// <summary>
    /// The signed-in caller, read from the validated token. Services use this instead
    /// of an id in the request body, so a caller can only ever act as themselves.
    /// </summary>
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }

        /// <summary>
        /// The caller's user id.
        /// Throws <see cref="UnauthorizedException"/> when there is no usable token,
        /// which should not happen behind [Authorize] but is not worth assuming.
        /// </summary>
        int UserId { get; }

        UserRole? Role { get; }

        bool IsAdmin { get; }
    }
}
