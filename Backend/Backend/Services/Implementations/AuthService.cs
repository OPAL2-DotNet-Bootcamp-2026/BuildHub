using Backend.Exceptions;
using Backend.Models.Dtos;
using Backend.Models.Entities;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Backend.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private const string InvalidCredentials = "Email or password is incorrect.";

        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ITokenService _tokenService;

        public AuthService(
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email.Trim());

            // One message for both failures. Saying "no such account" would turn this
            // endpoint into a way to enumerate who is registered.
            if (user is null)
            {
                throw new UnauthorizedException(InvalidCredentials);
            }

            var result = _passwordHasher.VerifyHashedPassword(
                user, user.PasswordHash, request.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedException(InvalidCredentials);
            }

            return _tokenService.CreateToken(user);
        }
    }
}
