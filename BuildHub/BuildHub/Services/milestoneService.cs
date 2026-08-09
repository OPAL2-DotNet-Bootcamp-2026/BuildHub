using BuildHub.DTOs;
using BuildHub.Repos;

namespace BuildHub.Services
{
    public class MilestoneService
    {
        private MilestoneRepo repo;

        public MilestoneService(MilestoneRepo _repo)
        {
            repo = _repo;
        }

        public List<MilestoneOutputDto> GetMilestonesByContractId(int contractId)
        {
            return repo.GetMilestonesByContractId(contractId)
                       .Select(m => new MilestoneOutputDto
                       {
                           MilestoneId = m.MilestoneId,
                           ContractId = m.ContractId,
                           Title = m.Title,
                           Amount = m.Amount,
                           OrderIndex = m.OrderIndex,
                           Status = m.Status,
                           DueDate = m.DueDate
                       })
                       .ToList();
        }

    }
}
