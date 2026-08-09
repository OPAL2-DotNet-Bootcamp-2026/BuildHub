using BuildHub.Models;

namespace BuildHub.Repos
{
    public class EscrowTransactionRepo
    {
        private ProjectContext context;

        public EscrowTransactionRepo(ProjectContext _context)
        {
            context = _context;
        }

        public void Add(EscrowTransaction escrowTransaction)
        {
            context.EscrowTransactions.Add(escrowTransaction);
            context.SaveChanges();
        }

        public EscrowTransaction GetByMilestoneId(int milestoneId)
        {
            return context.EscrowTransactions
                           .FirstOrDefault(e => e.MilestoneId == milestoneId);
        }

        public void Update()
        {
            context.SaveChanges();
        }
    }
}