namespace LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollmentHistory.Dtos
{
    public record EnrollmentHistoryResponseDto
    (
        int EnrollmentId,
        int TrackId,
        string TrackName,
        DateTime EnrollmentDate,
        string EnrollmentStatus
    );
}
