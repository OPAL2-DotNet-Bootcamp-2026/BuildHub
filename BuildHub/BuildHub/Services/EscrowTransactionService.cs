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

        public int OpenEscrowHold(int contractId, int milestoneId, decimal amount)
        {
            EscrowTransaction escrowTransaction = new EscrowTransaction();
            escrowTransaction.ContractId = contractId;
            escrowTransaction.MilestoneId = milestoneId;
            escrowTransaction.Amount = amount;
            escrowTransaction.Status = "Held"; // mocked hold
            escrowTransaction.HeldAt = DateTime.UtcNow;
            escrowTransaction.ReleasedAt = null;

            repo.Add(escrowTransaction);
            return escrowTransaction.EscrowTransactionId;
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

            escrowTransaction.Status = newStatus;

            if (newStatus == "Released")
            {
                escrowTransaction.ReleasedAt = DateTime.UtcNow;
            }

            repo.Update();
            return true;
        }
    }
}
