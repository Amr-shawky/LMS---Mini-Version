namespace LMS___Mini_Version.Messaging;

public record EnrollmentTransferred(Guid TransferId, int EnrollmentId, int PreviousTrackId, int NewTrackId, DateTime OccurredAtUtc);
