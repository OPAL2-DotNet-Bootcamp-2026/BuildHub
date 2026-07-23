
using System.ComponentModel.DataAnnotations;

namespace BuildHub.DTOs
{
    public class QuoteRequestInviteInputDTOs
    {
        [Required(ErrorMessage = "Value should not be null.")]
        public int QuoteRequestId { get; set; }

        [Required(ErrorMessage = "Value should not be null.")]
        public int VendorProfileId { get; set; }
    }

    public class QuoteRequestInviteOutputDTOs //
    {
        public int InviteId { get; set; }
        public int QuoteRequestId { get; set; }
        public int VendorProfileId { get; set; }
        public string InviteStatus { get; set; }
    }
}

 