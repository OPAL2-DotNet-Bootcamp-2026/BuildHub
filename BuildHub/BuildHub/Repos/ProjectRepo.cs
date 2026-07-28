using BuildHub.Models;

namespace BuildHub.Repos
{
    public class ProjectRepo
    {
        private ProjectContext _context;

        public ProjectRepo(ProjectContext context)
        {
            _context = context;
        }

        public void Add(Project project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();
        }
    }
}
