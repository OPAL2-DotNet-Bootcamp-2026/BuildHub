using Backend.Exceptions;
using Backend.Models.Dtos;

namespace Backend.Services.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Verifies the password and returns a bearer token.
        /// Throws <see cref="UnauthorizedException"/> when the email is unknown or the
        /// password is wrong - the same failure either way, so the response cannot be
        /// used to discover which addresses are registered.
        /// </summary>
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}
