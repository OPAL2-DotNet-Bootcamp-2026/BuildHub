using BuildHub.DTOs;
using BuildHub.Models;
using BuildHub.Repos;

namespace BuildHub.Services
{
    public class ContractService
    {
        private readonly ContractRepo _contractRepo;
        private readonly EscrowTransactionService _escrowService;

        public ContractService(ContractRepo contractRepo, EscrowTransactionService escrowService)
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
                ContractId = contract.ContractId,
                QuoteId = contract.QuoteId,
                TotalAmount = contract.TotalAmount,
                PaymentType = contract.PaymentType,
                Status = contract.Status,
                SignedAt = contract.SignedAt,

                Milestones = contract.Milestones?.Select(m => new MilestoneSummaryDto
                {
                    MilestoneId = m.MilestoneId,
                    Title = m.Title,
                    Amount = m.Amount,
                    DueDate = m.DueDate,
                    EndDate = m.EndDate,
                    Status = m.Status
                }).ToList(),

                EscrowTransactions = contract.EscrowTransactions?.Select(e => new EscrowTransactionOutputDto
                {
                    EscrowTransactionId = e.EscrowTransactionId,
                    Amount = e.Amount,
                    Status = e.Status,
                    HeldAt = e.HeldAt
                }).ToList()
            };
        }

        public void CreateContractOnQuoteAcceptance(int quoteId, decimal totalAmount, DateTime startDate, DateTime finishDate)
        {
            var newContract = new Contract
            {
                QuoteId = quoteId,
                TotalAmount = totalAmount,
                PaymentType = "OneTime",
                Status = "Active",
                SignedAt = DateTime.Now
            };

            var fullProjectMilestone = new Milestone
            {
                Title = "Full Project Milestone",
                Amount = totalAmount,
                DueDate = startDate,
                EndDate = finishDate,
                Status = "Pending"
            };

            newContract.Milestones = new List<Milestone> { fullProjectMilestone };

            _contractRepo.AddContract(newContract);

            _escrowService.OpenEscrowHold(newContract.ContractId, fullProjectMilestone.MilestoneId, totalAmount);
        }
    }
}
