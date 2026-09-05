using Backend.Models;
using Backend.Models.Entities;

namespace Backend.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();

        /// <summary>Returns null when no product has this id.</summary>
        Task<Product?> GetByIdAsync(int id);

        Task<Product> CreateAsync(Product product);

        /// <summary>
        /// Updates the listing. Returns null when the id does not exist.
        /// </summary>
        Task<Product?> UpdateAsync(int id, Product input);

        /// <summary>False when the id does not exist.</summary>
        Task<bool> DeleteAsync(int id);
    }
}
