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

        public List<Category> GetAll()
        {
            return repo.GetAll();
        }

        public Category GetById(int categoryId)
        {
            return repo.GetById(categoryId);
        }

        public int Add (Category category)
        {
            repo.Add(category);
            return category.categoryId;
        }
        
        public bool UpdateType (int categoryId , string newType)
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
