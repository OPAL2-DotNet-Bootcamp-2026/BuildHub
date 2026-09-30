using System.Text.Json.Serialization;

namespace Backend.Models.Thawani
{
    // ---- Request: POST /checkout/session ----

    public class ThawaniCreateSessionRequest
    {
        [JsonPropertyName("client_reference_id")]
        public string ClientReferenceId { get; set; } = string.Empty;

        [JsonPropertyName("mode")]
        public string Mode { get; set; } = "payment";

        [JsonPropertyName("products")]
        public List<ThawaniProduct> Products { get; set; } = new();

        [JsonPropertyName("success_url")]
        public string SuccessUrl { get; set; } = string.Empty;

        [JsonPropertyName("cancel_url")]
        public string CancelUrl { get; set; } = string.Empty;

        [JsonPropertyName("metadata")]
        public Dictionary<string, string>? Metadata { get; set; }
    }

    public class ThawaniProduct
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; } = 1;

        // Thawani amounts are in baisa (1 OMR = 1000 baisa), as an integer.
        [JsonPropertyName("unit_amount")]
        public int UnitAmount { get; set; }
    }

    // ---- Response envelope Thawani wraps every reply in ----

    public class ThawaniResponse<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }

    public class ThawaniSessionData
    {
        [JsonPropertyName("session_id")]
        public string SessionId { get; set; } = string.Empty;

        [JsonPropertyName("payment_status")]
        public string? PaymentStatus { get; set; } // "unpaid" | "paid" | "cancelled"

        [JsonPropertyName("client_reference_id")]
        public string? ClientReferenceId { get; set; }
    }

    // ---- Webhook payload: { "data": {...}, "event_type": "checkout.completed" } ----
    // "data"'s shape depends on event_type, so it's parsed on demand (see PaymentService).

    public class ThawaniWebhookEnvelope
    {
        [JsonPropertyName("event_type")]
        public string EventType { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public System.Text.Json.JsonElement Data { get; set; }
    }
}