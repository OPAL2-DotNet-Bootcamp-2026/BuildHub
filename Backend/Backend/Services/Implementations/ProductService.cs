using Backend.Exceptions;
using Backend.Models.Dtos;
using Backend.Models;
using Backend.Models.Entities;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;

namespace Backend.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IVendorProfileRepository _vendorProfileRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(
            IProductRepository productRepository,
            IVendorProfileRepository vendorProfileRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _vendorProfileRepository = vendorProfileRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<ProductResponse>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(ToResponse);
        }

        public async Task<ProductResponse?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product is null ? null : ToResponse(product);
        }

        public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
        {
            if (await _vendorProfileRepository.GetByIdAsync(request.VendorProfileId) is null)
            {
                throw new NotFoundException($"No vendor profile with id {request.VendorProfileId}.");
            }

            if (await _categoryRepository.GetByIdAsync(request.CategoryId) is null)
            {
                throw new NotFoundException($"No category with id {request.CategoryId}.");
            }

            var created = await _productRepository.CreateAsync(new Product
            {
                VendorProfileId = request.VendorProfileId,
                CategoryId = request.CategoryId,
                Name = request.Name.Trim(),
                Unit = request.Unit,
                Price = request.Price,
                ImageUrl = request.ImageUrl,
                IsAvailable = request.IsAvailable
            });

            return ToResponse(created);
        }

        public async Task<ProductResponse?> UpdateAsync(int id, UpdateProductRequest request)
        {
            if (await _categoryRepository.GetByIdAsync(request.CategoryId) is null)
            {
                throw new NotFoundException($"No category with id {request.CategoryId}.");
            }

            var updated = await _productRepository.UpdateAsync(id, new Product
            {
                Name = request.Name.Trim(),
                CategoryId = request.CategoryId,
                Unit = request.Unit,
                Price = request.Price,
                ImageUrl = request.ImageUrl,
                IsAvailable = request.IsAvailable
            });

            return updated is null ? null : ToResponse(updated);
        }

        public async Task<bool> DeleteAsync(int id) =>
            await _productRepository.DeleteAsync(id);

        private static ProductResponse ToResponse(Product product) => new()
        {
            ProductId = product.ProductId,
            VendorProfileId = product.VendorProfileId,
            CategoryId = product.CategoryId,
            Name = product.Name,
            Unit = product.Unit,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            IsAvailable = product.IsAvailable
        };
    }
}
