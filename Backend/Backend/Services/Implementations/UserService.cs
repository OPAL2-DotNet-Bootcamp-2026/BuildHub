using Backend.Exceptions;
using Backend.Models.Dtos;
using Backend.Models;
using Backend.Models.Entities;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<IEnumerable<UserResponse>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(ToResponse);
        }

        public async Task<UserResponse?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user is null ? null : ToResponse(user);
        }

        public async Task<UserResponse> CreateAsync(CreateUserRequest request)
        {
            var email = request.Email.Trim();

            // Checked up front so a taken email is a clear 409 rather than a raw
            // unique-index violation surfacing as a 500.
            if (await _userRepository.GetByEmailAsync(email) is not null)
            {
                throw new ConflictException($"The email '{email}' is already registered.");
            }

            var user = new User
            {
                FullName = request.FullName.Trim(),
                Email = email,
                PhoneNumber = request.PhoneNumber,
                Role = request.Role,
                City = request.City.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            // The hash needs the entity, so it is set after construction.
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            try
            {
                var created = await _userRepository.CreateAsync(user);
                return ToResponse(created);
            }
            catch (DbUpdateException)
            {
                // Two registrations for the same email can pass the check above and
                // race to the insert; the unique index is what actually decides.
                throw new ConflictException($"The email '{email}' is already registered.");
            }
        }

        public async Task<UserResponse?> UpdateAsync(int id, UpdateUserRequest request)
        {
            // Only the three editable fields are carried across - the repository
            // ignores everything else, so email, password and role cannot move.
            var input = new User
            {
                FullName = request.FullName.Trim(),
                PhoneNumber = request.PhoneNumber,
                City = request.City.Trim()
            };

            var updated = await _userRepository.UpdateAsync(id, input);
            return updated is null ? null : ToResponse(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _userRepository.DeleteAsync(id);
            }
            catch (DbUpdateException)
            {
                // Job.HomeownerId and Review.ReviewerId are Restrict: a user who has
                // posted work or left a rating cannot be erased out from under it.
                throw new ConflictException(
                    "This user cannot be deleted while they still have jobs or reviews.");
            }
        }

        private static UserResponse ToResponse(User user) => new()
        {
            UserId = user.UserId,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role,
            City = user.City,
            CreatedAt = user.CreatedAt
        };
    }
}
