using Backend.Data;
using Backend.Models;
using Backend.Models.Entities;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BuildHubDbContext _context;

        public CategoryRepository(BuildHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync() =>
            await _context.Categories.AsNoTracking().ToListAsync();

        public async Task<Category?> GetByIdAsync(int id) =>
            await _context.Categories.AsNoTracking()
                .FirstOrDefaultAsync(c => c.CategoryId == id);

        public async Task<Category> CreateAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> UpdateAsync(int id, Category input)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category is null) return null;

            // A category is only a label, so every field on it is editable.
            category.NameAr = input.NameAr;
            category.NameEn = input.NameEn;
            category.IconUrl = input.IconUrl;

            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category is null) return false;

            // Throws DbUpdateException while any vendor, job or product still uses it -
            // deleting a category must never wipe everything filed under it.
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
