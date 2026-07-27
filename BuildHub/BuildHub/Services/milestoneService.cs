using BuildHub.DTOs;
using BuildHub.Repos;
using Microsoft.VisualBasic;
using static BuildHub.DTOs.MilstoneDto;

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
                           milestoneId = m.milestoneId,
                           contractId = m.contractId,
                           title = m.title,
                           amount = m.amount,
                           orderIndex = m.orderIndex,
                           status = m.status,
                           dueDate =  m.DueDate
                       })
                       .ToList();
        }

    }
}
