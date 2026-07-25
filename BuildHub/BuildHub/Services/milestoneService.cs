using BuildHub.DTOs;
using BuildHub.Repos;
using Microsoft.VisualBasic;
using static BuildHub.DTOs.milstoneDto;

namespace BuildHub.Services
{
    public class MilestoneService
    {
        private milestoneRepo repo;

        public MilestoneService(milestoneRepo _repo)
        {
            repo = _repo;
        }

        public List<MilestoneOutputDto> GetMilestonesByContractId(int contractId)
        {
            return repo.GetMilestonesByContractId(contractId)
                       .Select(m => new MilestoneOutputDto
                       {
                           milestoneId = m.milestoneId,
                           contractId = m.contractId,
                           title = m.title,
                           amount = m.amount,
                           orderIndex = m.orderIndex,
                           status = m.status,
                           dueDate =  m.dueDate
                       })
                       .ToList();
        }

    }
}
