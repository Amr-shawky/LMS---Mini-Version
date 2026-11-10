namespace LMS___Mini_Version.features.EnrollmentFeature.GetEnrollmentTrackIntern.DTOS
{
    public record GetEnrollmentTrackInternDTO(int EnrollmentId, string InternFullName, string EnrollmentDate, string EnrollmentStatus, decimal? PaymentAmount, string? PaymentStatus);
    // EnrollmentId, InternFullName, EnrollmentDate, EnrollmentStatus, PaymentAmount, and
    //PaymentStatus
}
