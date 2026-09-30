namespace Backend.Models
{
    // Starts at 1, following this project's convention (see the enums file):
    // status fields have no DB default, so 0 stays unused and a forgotten
    // assignment never silently reads as a valid status.
    public enum OrderStatus
    {
        Pending = 1,
        Paid = 2,
        Failed = 3,
        Cancelled = 4
    }
}

namespace Backend.Models.Entities
{
    using Backend.Models;

    public class Order
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        // Adjust the type to match however you identify the logged-in user
        // (e.g. string for Identity's default Guid-as-string user id).
        public string UserId { get; set; } = string.Empty;

        public decimal Amount { get; set; } // stored in OMR, e.g. 14.500

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        // Set once we create a Thawani checkout session for this order.
        public string? ThawaniSessionId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; }
    }
}