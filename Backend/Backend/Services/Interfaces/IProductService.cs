using Backend.Exceptions;
using Backend.Models.Dtos;

namespace Backend.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponse>> GetAllAsync();

        /// <summary>Null when no product has this id.</summary>
        Task<ProductResponse?> GetByIdAsync(int id);

        /// <summary>
        /// Lists a material item.
        /// Throws <see cref="NotFoundException"/> when the vendor or category does not exist.
        /// </summary>
        Task<ProductResponse> CreateAsync(CreateProductRequest request);

        /// <summary>
        /// Null when no product has this id.
        /// Throws <see cref="NotFoundException"/> when the category does not exist.
        /// </summary>
        Task<ProductResponse?> UpdateAsync(int id, UpdateProductRequest request);

        /// <summary>False when no product has this id.</summary>
        Task<bool> DeleteAsync(int id);
    }
}
