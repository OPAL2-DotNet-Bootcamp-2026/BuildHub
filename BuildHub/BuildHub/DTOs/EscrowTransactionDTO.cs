using System.ComponentModel.DataAnnotations;

namespace BuildHub.DTOs
{
    public class EscrowTransactionOutputDTO
    {
        public int EscrowTransactionId { get; set; }
        public int ContractId { get; set; }
        public int? MilestoneId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime? HeldAt { get; set; }
        public DateTime? ReleasedAt { get; set; }
    }

    public class EscrowTransactionInputDTO
    {
        [Required(ErrorMessage = "Value should not be null.")]
        public int ContractId { get; set; }

        [Required(ErrorMessage = "Value should not be null.")]
        public int MilestoneId { get; set; }

        [Required(ErrorMessage = "Value should not be null.")]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }
    }





}
