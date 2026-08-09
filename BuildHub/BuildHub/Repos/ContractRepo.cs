using BuildHub.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildHub.Repos
{
    public class ContractRepo
    {
        private ProjectContext context;

        public ContractRepo(ProjectContext _context)
        {
            context = _context;
        }

        public Contract GetContractByIdWithDetails(int id)
        {
            return context.Contracts
                .Include(c => c.Milestones)
                .Include(c => c.EscrowTransactions)
                .FirstOrDefault(c => c.ContractId == id);
        }

        public List<Contract> GetAllContracts()
        {
            return context.Contracts.ToList();
        }

        public void AddContract(Contract contract)
        {
            context.Contracts.Add(contract);
            context.SaveChanges();
        }

        public void Update()
        {
            context.SaveChanges();
        }

        public void Delete(Contract contract)
        {
            context.Contracts.Remove(contract);
            context.SaveChanges();
        }
    }
}
