using Backend.Exceptions;
using Backend.Models.Dtos;

namespace Backend.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> GetAllAsync();

        /// <summary>Null when no category has this id.</summary>
        Task<CategoryResponse?> GetByIdAsync(int id);

        Task<CategoryResponse> CreateAsync(CreateCategoryRequest request);

        /// <summary>Null when no category has this id.</summary>
        Task<CategoryResponse?> UpdateAsync(int id, UpdateCategoryRequest request);

        /// <summary>
        /// False when no category has this id.
        /// Throws <see cref="ConflictException"/> when vendors, jobs or products still use it.
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}
