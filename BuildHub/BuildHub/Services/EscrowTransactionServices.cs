using BuildHub.Models;
using BuildHub.Repos;

namespace BuildHub.Services
{
    public class EscrowTransactionService
    {
        //EscrowTransactionRepo repo = new EscrowTransactionRepo();
        //apply dependency inversion 
        private EscrowTransactionRepo repo;

        public EscrowTransactionService(EscrowTransactionRepo _repo)
        {
            repo = _repo;
        }
        
    }
}