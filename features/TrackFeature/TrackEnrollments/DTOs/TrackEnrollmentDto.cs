namespace LMS___Mini_Version.features.TrackFeature.TrackEnrollments.DTOs;

public record TrackEnrollmentDto(
    int EnrollmentId,
    string InternFullName,
    DateTime EnrollmentDate,
    string EnrollmentStatus,
    decimal? PaymentAmount,
    string? PaymentStatus);