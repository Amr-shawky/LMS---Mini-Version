namespace LMS___Mini_Version.features.EnrollmentFeature.DTOs;

public record EnrollmentDto(
    int EnrollmentId,
    int TrackId,
    string TrackName,
    DateTime EnrollmentDate,
    string EnrollmentStatus);