using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Dtos
{
    public class CreatePaymentCheckoutRequest
    {
        // Frontend sends the agreement that needs payment
        [Range(1, int.MaxValue)]
        public int AgreementId { get; set; }
    }

}
