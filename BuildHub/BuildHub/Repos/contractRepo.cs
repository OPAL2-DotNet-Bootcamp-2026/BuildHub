
using BuildHub.Models;

namespace BuildHub.Repos
{
    public class contractRepo
    {
        private ProjectContext context;

        public contractRepo(ProjectContext _context)
        {
            context = _context;   
        }



        public List<Contract> GetAllContracts()
        {
            return context.Contracts.ToList();
        }


        public Contract GetAllContractById(int id)

        { 
            return context.Contracts.FirstOrDefault(c => c.contractId == id);
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
