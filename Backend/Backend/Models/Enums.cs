namespace Backend.Models
{
    // Every enum starts at 1, never 0. "Status fields have no DB defaults - set them
    // explicitly on every insert": with 0 unused, a forgotten assignment lands on an
    // undefined value instead of silently meaning Open / Pending / Held.

    public enum UserRole
    {
        Homeowner = 1,
        Vendor = 2,
        Admin = 3
    }

    public enum VendorType
    {
        Contractor = 1,
        Designer = 2,
        Store = 3
    }

    public enum JobStatus
    {
        Open = 1,
        Hired = 2,
        Completed = 3,
        Cancelled = 4
    }

    public enum OfferStatus
    {
        Pending = 1,
        Accepted = 2,
        NotSelected = 3
    }

    public enum AgreementStatus
    {
        Active = 1,
        Completed = 2,
        Cancelled = 3
    }

    /// <summary>Escrow state. Mocked - a state machine, not a payment integration.</summary>
    public enum PaymentStatus
    {
        Held = 1,
        Released = 2,
        Refunded = 3
    }

    public enum ProductUnit
    {
        SquareMeter = 1,
        Piece = 2,
        Set = 3
    }

    /// <summary>
    /// What a notification is about. Determines what
    /// <see cref="Entities.Notification.RelatedId"/>
    /// points at, so the client can deep-link.
    /// </summary>
    public enum NotificationType
    {
        /// <summary>A vendor submitted an offer. RelatedId = offerId.</summary>
        OfferReceived = 1,

        /// <summary>The homeowner accepted this vendor's offer. RelatedId = offerId.</summary>
        OfferAccepted = 2,

        /// <summary>Another offer won the job. RelatedId = offerId.</summary>
        OfferNotSelected = 3,

        /// <summary>Agreement created, money held in escrow. RelatedId = agreementId.</summary>
        AgreementStarted = 4,

        /// <summary>Homeowner confirmed the work is done. RelatedId = jobId.</summary>
        JobCompleted = 5,

        /// <summary>Escrow released to the vendor's balance. RelatedId = agreementId.</summary>
        PaymentReleased = 6,

        /// <summary>An admin refunded the homeowner. RelatedId = agreementId.</summary>
        PaymentRefunded = 7,

        /// <summary>A review was left on the vendor. RelatedId = reviewId.</summary>
        ReviewReceived = 8
    }
}
