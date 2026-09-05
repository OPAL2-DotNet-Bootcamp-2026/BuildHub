using Backend.Exceptions;
using Backend.Models.Dtos;
using Backend.Models;
using Backend.Models.Entities;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(ToResponse);
        }

        public async Task<CategoryResponse?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category is null ? null : ToResponse(category);
        }

        public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request)
        {
            var created = await _categoryRepository.CreateAsync(new Category
            {
                NameAr = request.NameAr.Trim(),
                NameEn = request.NameEn.Trim(),
                IconUrl = request.IconUrl
            });

            return ToResponse(created);
        }

        public async Task<CategoryResponse?> UpdateAsync(int id, UpdateCategoryRequest request)
        {
            var updated = await _categoryRepository.UpdateAsync(id, new Category
            {
                NameAr = request.NameAr.Trim(),
                NameEn = request.NameEn.Trim(),
                IconUrl = request.IconUrl
            });

            return updated is null ? null : ToResponse(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _categoryRepository.DeleteAsync(id);
            }
            catch (DbUpdateException)
            {
                throw new ConflictException(
                    "This category cannot be deleted while vendors, jobs or products still use it.");
            }
        }

        private static CategoryResponse ToResponse(Category category) => new()
        {
            CategoryId = category.CategoryId,
            NameAr = category.NameAr,
            NameEn = category.NameEn,
            IconUrl = category.IconUrl
        };
    }
}
