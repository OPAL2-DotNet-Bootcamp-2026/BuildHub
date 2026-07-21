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

    }
}
