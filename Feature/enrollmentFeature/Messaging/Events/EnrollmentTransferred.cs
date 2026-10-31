namespace LMS___Mini_Version.Feature.enrollmentFeature.Messaging.Events
{
    /// <summary>
    /// Published when an enrollment is successfully transferred to a new track.
    ///
    /// Fields (D3): the minimal set both publisher and consumers need.
    /// OccurredAtUtc captures when the transfer happened in the database transaction,
    /// not when the message is delivered (which can be later due to the outbox).
    /// </summary>
    public record EnrollmentTransferred(
        int EnrollmentId,
        int InternId,
        int OldTrackId,
        int NewTrackId,
        DateTime OccurredAtUtc);
}