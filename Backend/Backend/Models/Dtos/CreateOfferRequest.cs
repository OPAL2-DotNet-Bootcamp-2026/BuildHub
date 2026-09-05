using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Dtos
{
    /// <summary>
    /// A vendor's quote on a job. Status is absent - a new offer is always Pending.
    /// VendorProfileId is absent: the offer comes from the profile belonging to the
    /// signed-in vendor, so one vendor cannot bid in another's name.
    /// </summary>
    public class CreateOfferRequest
    {
        [Range(1, int.MaxValue)]
        public int JobId { get; set; }

        [Range(0, 9999999999999.999)]
        public decimal Price { get; set; }

        [Range(1, 3650)]
        public int DurationDays { get; set; }

        [MaxLength(2000)]
        public string? Message { get; set; }
    }
}
