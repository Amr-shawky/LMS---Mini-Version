namespace LMS___Mini_Version.Messages
{
    public record EnrollmentTransferred(
        int InternId,
        int PreviousEnrollmentId,
        int PreviousTrackId,
        int NewTrackId,
        DateTime TransferDate
        );
}
