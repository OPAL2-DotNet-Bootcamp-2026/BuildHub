
namespace Backend.Models.Dtos
{
    public class AgreementResponse
    {
        public int AgreementId { get; set; }

        public int OfferId { get; set; }

        public decimal TotalAmount { get; set; }

        public AgreementStatus Status { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public DateTime? HeldAt { get; set; }

        public DateTime? ReleasedAt { get; set; }

        public DateTime StartedAt { get; set; }
    }
}
