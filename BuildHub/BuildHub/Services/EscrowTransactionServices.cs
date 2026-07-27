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

        public int Create(int contractId, int milestoneId, decimal amount)
        {
            EscrowTransaction escrowTransaction = new EscrowTransaction();
            escrowTransaction.contractId = contractId;
            escrowTransaction.milestoneId = milestoneId;
            escrowTransaction.amount = amount;
            escrowTransaction.status = "Held"; // mocked hold
            escrowTransaction.heldAt = DateTime.UtcNow;
            escrowTransaction.releasedAt = null;

            repo.Add(escrowTransaction);
            return escrowTransaction.escrowTransactionId;
        }

        public EscrowTransaction GetByMilestoneId(int milestoneId)
        {
            return repo.GetByMilestoneId(milestoneId);
        }

        public bool UpdateStatus(int milestoneId, string newStatus)
        {
            EscrowTransaction escrowTransaction = repo.GetByMilestoneId(milestoneId);
            if (escrowTransaction == null)
            {
                return false;
            }

            escrowTransaction.status = newStatus;

            if (newStatus == "Released")
            {
                escrowTransaction.releasedAt = DateTime.UtcNow;
            }

            repo.Update();
            return true;
        }
    }
}