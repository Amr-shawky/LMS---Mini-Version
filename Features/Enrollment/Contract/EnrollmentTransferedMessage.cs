namespace LMS___Mini_Version.Features.Enrollment.Contract
{
    public record EnrollmentTransferedMessage(
        int EnrollmentId,
        int OldTrackId,
        int NewTrackId,
        int InternId
    );
}
