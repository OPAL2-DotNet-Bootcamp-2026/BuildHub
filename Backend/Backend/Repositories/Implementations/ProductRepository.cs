using Backend.Data;
using Backend.Models;
using Backend.Models.Entities;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly BuildHubDbContext _context;

        public ProductRepository(BuildHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync() =>
            await _context.Products.AsNoTracking().ToListAsync();

        public async Task<Product?> GetByIdAsync(int id) =>
            await _context.Products.AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductId == id);

        public async Task<Product> CreateAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateAsync(int id, Product input)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null) return null;

            // The whole listing is editable - a product is a price-comparison entry,
            // never referenced by a job, offer or agreement, so nothing depends on it.
            product.Name = input.Name;
            product.CategoryId = input.CategoryId;
            product.Unit = input.Unit;
            product.Price = input.Price;
            product.ImageUrl = input.ImageUrl;
            product.IsAvailable = input.IsAvailable;

            // Deliberately not updated here:
            //   VendorProfileId - which store owns the listing; reassigning it would
            //                     move another vendor's product into someone else's shelf

            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
