using BuildHub.DTOs;
using BuildHub.Models;
using BuildHub.Repos;
using static BuildHub.DTOs.contractDto;

namespace BuildHub.Services
{
    public class ContractService
    {
        private readonly contractRepo _contractRepo;
        private readonly EscrowTransactionService _escrowService; 

        public ContractService(contractRepo contractRepo, EscrowTransactionService escrowService)
        {
            _contractRepo = contractRepo;
            _escrowService = escrowService;
        }

        public ContractDetailsOutputDto GetById(int id)
        {
            var contract = _contractRepo.GetContractByIdWithDetails(id);
            if (contract == null) return null;

            return new ContractDetailsOutputDto
            {
                contractId = contract.contractId,
                quoteId = contract.quoteId,
                totalAmount = contract.totalAmount,
                paymentType = contract.paymentType,
                status = contract.status,
                signedAt = contract.signedAt,

                milstones = contract.Milestones?.Select(m => new MilstoneDto
                {
                    milestoneId = m.milestoneId,
                    title = m.title,
                    amount = m.amount,
                    DueDate = m.DueDate, 
                    endDate = m.endDate,
                    status = m.status
                }).ToList(),


                escrowTransactions = contract.EscrowTransactions?.Select(e => new EscrowTransactionOutputDTO
                {
                    escrowTransactionId = e.escrowTransactionId,
                    amount = e.amount,
                    status = e.status,
                    heldAt = e.heldAt
                }).ToList()
            };
        }

        public void CreateContractOnQuoteAcceptance(int quoteId, decimal totalAmount, DateTime startDate, DateTime finishDate)
        {
            var newContract = new Contract
            {
                quoteId = quoteId,
                totalAmount = totalAmount,
                paymentType = "OneTime",
                status = "Active",
                signedAt = DateTime.Now
            };

            _contractRepo.AddContract(newContract);
            
            var fullProjectMilestone = new Milestone
            {
                title = "Full Project Milestone",
                amount = totalAmount,
                DueDate = startDate,
                endDate = finishDate,
                status = "Pending"
            };

            newContract.Milestones = new List<Milestone> { fullProjectMilestone };

            _contractRepo.AddContract(newContract);

            _escrowService.OpenEscrowHold(fullProjectMilestone.milestoneId, totalAmount);
        }
    }
}
