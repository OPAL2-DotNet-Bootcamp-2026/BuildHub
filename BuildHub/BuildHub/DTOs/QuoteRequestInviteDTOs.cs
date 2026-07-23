
namespace BuildHub.DTOs
{
    public class QuoteRequestInviteInputDTOs
    {
        [Required(ErrorMessage = "Value should not be null.")]
        public int QuoteRequestId { get; set; }

        [Required(ErrorMessage = "Value should not be null.")]
        public int VendorProfileId { get; set; }
    }
    
}

 