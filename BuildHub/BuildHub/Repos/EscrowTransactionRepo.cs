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
    }
}