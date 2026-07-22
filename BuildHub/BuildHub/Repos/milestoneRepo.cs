using BuildHub.Models;

namespace BuildHub.Repos
{
    public class milestoneRepo
    {
        private ProjectContext context;


        public milestoneRepo(ProjectContext _context)
        {
            context = _context;

        }
        
        public List<Milestone> GetAllMilestones()
        {
            return context.Milestones.ToList();
        }

        public Milestone GetAllMilestoneById(int id)

        { 
            return context.Milestones.FirstOrDefault(m => m.milestoneId == id);

        }
        public void AddMilestone(Milestone milestone)
        { 
            context.Milestones.Add(milestone);
            context.SaveChanges();
        }

        public void Ubdate()
        {
            context.SaveChanges();
        }

        public void Delete (Milestone milestone)
        {
            context.Milestones.Remove(milestone);
            context.SaveChanges() ;
        }






    }
}
