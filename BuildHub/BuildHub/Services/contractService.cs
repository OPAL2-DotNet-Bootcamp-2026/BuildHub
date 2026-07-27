using BuildHub.DTOs;
using BuildHub.Models;
using BuildHub.Repos;
using static BuildHub.DTOs.contractDto;

namespace BuildHub.Services
{
    public class ContractService
    {
        private readonly contractRepo _contractRepo;
        private readonly EscrowService _escrowService; 

        public ContractService(contractRepo contractRepo, EscrowService escrowService)
        {
            _contractRepo = contractRepo;
            _escrowService = escrowService;
        }

        public ContractDeatailsOutputDto GetById(int id)
        {
            var contract = _contractRepo.GetContractByIdWithDetails(id);
            if (contract == null) return null;

            return new ContractDeatailsOutputDto
            {
                contractId = contract.contractId,
                quoteId = contract.quoteId,
                totalAmount = contract.totalAmount,
                paymentType = contract.paymentType,
                status = contract.status,
                signedAt = contract.signedAt,

                milestones = contract.Milestones?.Select(m => new MilstoneDto
                {
                    milestoneId = m.milestoneId,
                    title = m.title,
                    amount = m.amount,
                    startDate = m.startDate,
                    endDate = m.endDate,
                    status = m.status
                }).ToList(),

                escrowTransactions = contract.EscrowTransactions?.Select(e => new EscrowTransactionDto
                {
                    transactionId = e.transactionId,
                    amount = e.amount,
                    status = e.status,
                    createdAt = e.createdAt
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

            var fullProjectMilestone = new Milestone
            {
                title = "Full Project Milestone",
                amount = totalAmount,
                startDate = startDate,
                endDate = finishDate,
                status = "Pending"
            };

            newContract.Milestones = new List<Milestone> { fullProjectMilestone };

            _contractRepo.AddContract(newContract);

            _escrowService.OpenEscrowHold(fullProjectMilestone.milestoneId, totalAmount);
        }
    }
}
