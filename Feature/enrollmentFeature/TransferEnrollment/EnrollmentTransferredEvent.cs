namespace LMS___Mini_Version.Feature.enrollmentFeature.TransferEnrollment
{
    public record EnrollmentTransferredEvent(
        int EnrollmentId,
        int InternId,
        int OldTrackId,
        int NewTrackId,
        DateTime TransferredAt
    );
}

