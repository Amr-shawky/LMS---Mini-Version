namespace LMS___Mini_Version.Domain.Entities
{
    /// <summary>
    /// Records each successful refund event.
    ///
    /// Why inherit baseEntity?
    ///   baseEntity provides the int Id primary key that every table in this project uses.
    ///   We do NOT add another Id field -- it would clash with the inherited one.
    ///
    /// Fields chosen:
    ///   PaymentId       -- links to the Payment that was refunded (for audit)
    ///   EnrollmentId    -- links to the Enrollment (convenient lookup without joining Payment)
    ///   PreviousStatus  -- the payment status BEFORE the refund (always Completed for eligible refunds)
    ///   NewStatus       -- the payment status AFTER the refund (always Refunded)
    ///   RefundedAt      -- the UTC timestamp when the refund was recorded
    /// </summary>
    public class RefundHistory : baseEntity
    {
        public int PaymentId { get; set; }
        public Payment Payment { get; set; } = null!;

        public int EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; } = null!;

        public string PreviousStatus { get; set; } = string.Empty;
        public string NewStatus { get; set; } = string.Empty;

        public DateTime RefundedAt { get; set; }
    }
}