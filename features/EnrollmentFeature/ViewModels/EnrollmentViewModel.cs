namespace LMS___Mini_Version.features.EnrollmentFeature.ViewModels;

public record EnrollmentViewModel(
    int EnrollmentId,
    int TrackId,
    string TrackName,
    DateTime EnrollmentDate,
    string EnrollmentStatus);