using Backend.Models;
using Backend.Models.Entities;

namespace Backend.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();

        /// <summary>Returns null when no category has this id.</summary>
        Task<Category?> GetByIdAsync(int id);

        Task<Category> CreateAsync(Category category);

        /// <summary>
        /// Updates the display names and icon. Returns null when the id does not exist.
        /// </summary>
        Task<Category?> UpdateAsync(int id, Category input);

        /// <summary>False when the id does not exist.</summary>
        Task<bool> DeleteAsync(int id);
    }
}
