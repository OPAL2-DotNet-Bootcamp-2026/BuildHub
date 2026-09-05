using Backend.Exceptions;
using Backend.Models.Dtos;

namespace Backend.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetAllAsync();

        /// <summary>Null when no user has this id, so the controller can answer 404.</summary>
        Task<UserResponse?> GetByIdAsync(int id);

        /// <summary>
        /// Hashes the password and stores the user.
        /// Throws <see cref="ConflictException"/> when the email is already registered.
        /// </summary>
        Task<UserResponse> CreateAsync(CreateUserRequest request);

        /// <summary>
        /// Updates the editable profile details.
        /// Null when no user has this id.
        /// </summary>
        Task<UserResponse?> UpdateAsync(int id, UpdateUserRequest request);

        /// <summary>
        /// False when no user has this id.
        /// Throws <see cref="ConflictException"/> when the user still has jobs or reviews.
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}
