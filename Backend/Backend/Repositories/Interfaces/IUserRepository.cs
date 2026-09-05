using Backend.Models;
using Backend.Models.Entities;

namespace Backend.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();

        /// <summary>Returns null when no user has this id, so the caller can answer 404.</summary>
        Task<User?> GetByIdAsync(int id);

        /// <summary>
        /// Looks a user up by email, which is unique. Returns null when nobody uses it.
        /// Matching is case-insensitive, following the database collation.
        /// </summary>
        Task<User?> GetByEmailAsync(string email);

        Task<User> CreateAsync(User user);

        /// <summary>
        /// Updates the editable profile details only.
        /// Returns null when the id does not exist.
        /// </summary>
        Task<User?> UpdateAsync(int id, User input);

        /// <summary>False when the id does not exist.</summary>
        Task<bool> DeleteAsync(int id);
    }
}
