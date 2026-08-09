using System.ComponentModel.DataAnnotations;

namespace BuildHub.DTOs
{
    // Input
    public class CreateMilestoneDto
    {
        [Required(ErrorMessage = "Contract Id is required .")]
        public int ContractId { get; set; }

        [Required(ErrorMessage = "Titel is required .")]
        public string Title { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Should be more than 0 ")]
        public decimal Amount { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Should be more than 0")]
        public int OrderIndex { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
    }


    public class UpdateMilestoneDto
    {
        [Required]
        public DateTime DueDate { get; set; }
    }


    // Output
    public class MilestoneOutputDto
    {
        public int MilestoneId { get; set; }
        public int ContractId { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public int OrderIndex { get; set; }
        public string Status { get; set; }
        public DateTime? DueDate { get; set; }
    }


    // Milestone as it appears nested inside ContractDetailsOutputDto
    public class MilestoneSummaryDto
    {
        public int MilestoneId { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
    }
}
