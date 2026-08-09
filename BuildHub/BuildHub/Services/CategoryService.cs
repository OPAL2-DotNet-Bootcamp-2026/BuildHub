using BuildHub.DTOs;
using BuildHub.Models;
using BuildHub.Repos;

namespace BuildHub.Services
{
    public class CategoryService
    {
        private CategoryRepo repo;
        public CategoryService(CategoryRepo _repo)
        {
            repo = _repo;
        }

        // Function to get all the data
        public List<Category> GetAll()
        {
            return repo.GetAll();
        }

        // Function to get by ID only
        public Category GetById(int categoryId)
        {
            return repo.GetById(categoryId);
        }

        // Function used to add or create
        public int Add(CategoryInputDto categoryDto)
        {
            Category category = new Category()
            {
                NameAr = categoryDto.NameAr,
                NameEn = categoryDto.NameEn,
                Type = categoryDto.Type,
            };

            repo.Add(category);
            return category.CategoryId;
        }

        // Function to update
        public bool UpdateType(int categoryId, string newType)
        {
            Category category = repo.GetById(categoryId);
            if (category == null)
            {
                return false;
            }
            category.Type = newType;
            repo.Update();
            return true;
        }

        // Function to delete
        public bool Delete(int categoryId)
        {
            Category category = repo.GetById(categoryId);
            if (category == null)
            {
                return false;
            }
            repo.Delete(category);
            return true;
        }
    }
}
