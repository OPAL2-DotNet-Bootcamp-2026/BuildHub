using System.ComponentModel.DataAnnotations;

namespace BuildHub.DTOs
{
    // Input
    public class ContractInputDto
    {
        [Required]
        public int QuoteId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Should be more than 0")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Payment Type is required .")]
        [AllowedValues("One time ", "PreMilestone", ErrorMessage = "Invalid payment type")]
        public string PaymentType { get; set; }
    }


    // Output
    public class ContractOutputDto
    {
        public int ContractId { get; set; }
        public int QuoteId { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentType { get; set; }
        public string Status { get; set; }
        public DateTime SignedAt { get; set; }
    }


    public class ContractDetailsOutputDto
    {
        public int ContractId { get; set; }
        public int QuoteId { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentType { get; set; }
        public string Status { get; set; }
        public DateTime SignedAt { get; set; }

        public List<MilestoneSummaryDto> Milestones { get; set; }
        public List<EscrowTransactionOutputDto> EscrowTransactions { get; set; }
    }


    public class UpdateContractStatusDto
    {
        public string Status { get; set; }
    }
}
