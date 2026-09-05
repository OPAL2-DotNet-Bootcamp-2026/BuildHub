using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Dtos
{
    /// <summary>
    /// Correcting a quote before anyone acts on it. The data model allows no revisions
    /// once an offer is decided, so the service accepts this only while it is Pending.
    /// </summary>
    public class UpdateOfferRequest
    {
        [Range(0, 9999999999999.999)]
        public decimal Price { get; set; }

        [Range(1, 3650)]
        public int DurationDays { get; set; }

        [MaxLength(2000)]
        public string? Message { get; set; }
    }
}
