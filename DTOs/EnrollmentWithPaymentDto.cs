namespace LMS___Mini_Version.DTOs
{
    /// <summary>
    /// Holds both Enrollment and optional Payment data returned by the Enroll orchestrator.
    /// Replaces the old EnrollmentResultDto's data fields.
    /// </summary>
    public class EnrollmentWithPaymentDto
    {
        public EnrollmentDto Enrollment { get; set; } = null!;
        public PaymentDto? Payment { get; set; }
    }
}
