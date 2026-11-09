namespace LMS___Mini_Version.features.TrackFeature.TrackEnrollments.ViewModel;

public record TrackEnrollmentViewModel(
    int EnrollmentId,
    string InternFullName,
    DateTime EnrollmentDate,
    string EnrollmentStatus,
    decimal? PaymentAmount,
    string? PaymentStatus);