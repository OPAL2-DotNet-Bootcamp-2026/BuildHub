using BuildHub.Models;

namespace BuildHub.Repos
{
    public class MilestoneRepo
    {
        private ProjectContext context;


        public MilestoneRepo(ProjectContext _context)
        {
            context = _context;

        }

        public void AddMilestone(Milestone milestone)
        {
            context.Milestones.Add(milestone);
            context.SaveChanges();
        }




        /*
        public List<Milestone> GetAllMilestones()
        {
            return context.Milestones.ToList();
        }

        */





        public List<Milestone> GetMilestonesByContractId(int contractId)
        {
            return context.Milestones.Where(m => m.ContractId == contractId).ToList();
        }
        public Milestone GetAllMilestoneById(int id)

        { 
            return context.Milestones.FirstOrDefault(m => m.MilestoneId == id);

        }
        
        public void Update()
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
