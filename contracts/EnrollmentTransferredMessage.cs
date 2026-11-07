namespace LMS___Mini_Version.contracts
{
    public record EnrollmentTransferredMessage
    {
        public int EnrollmentId { get; init; }
        public int OldTrackId { get; init; }
        public int NewTrackId { get; init; }
        public DateTime OccurredAt { get; init; }
    }
}
