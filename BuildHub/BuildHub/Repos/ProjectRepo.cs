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

        public List<Project> GetAll( )
        {
            return _context.Projects.ToList();
        }

        public Project Add(int id)
        {
            Project project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
            return project;
        }

        public void Add(Project project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();
        }
    }
}
