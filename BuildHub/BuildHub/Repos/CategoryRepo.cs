using BuildHub.Models;

namespace BuildHub.Repos
{
    public class CategoryRepo
    {
        private ProjectContext context;

        public CategoryRepo(ProjectContext context)
        {
            this.context = context;
        }

        public List<Category> GetAll()
        {
            return context.Categories.ToList();
        }

        public Category GetById(int categoryId)
        {
            return context.Categories.FirstOrDefault(c => c.categoryId == categoryId);
        }

        public void Add (Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
        }

        public void Update()
        {
            context.SaveChanges();
        }
    }
}
