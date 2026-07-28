using BuildHub.DTOs;
using BuildHub.Models;
using BuildHub.Repos;

namespace BuildHub.Services
{
    public class CategoryService
    {
        private CategoryRepo repo;
        public CategoryService (CategoryRepo _repo)
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
        public int Add(CategoryInputDTOs categoryDTO)
        {
            Category category = new Category()
            {
                nameAr = categoryDTO.nameAr,
                nameEn = categoryDTO.nameEn,
                type = categoryDTO.type,
            };

            repo.Add(category);
            return category.categoryId;
        }
        
        // Function to update 
        public bool UpdateType(int categoryId , string newType)
        {
            Category category = repo.GetById(categoryId);
            if (category == null)
            {
                return false;
            }
            category.type = newType;
            repo.Update();
            return true;
        }

        // Function to delete
        public bool Delete (int categoryId)
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
