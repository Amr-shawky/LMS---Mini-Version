namespace LMS___Mini_Version.features.shared
{
    public enum ErrorCode
    {
        None,
        InternNotFound=1,
        InternAlreadyExists = 2,
        InternHasEnrollment = 3,
        InternHasNoEnrollment = 4,
        InternTrackNotActive = 5,
        InternHasNoPendingEnrollment = 6,
        TrackNotFound =1000,
        TrackIsNotActive = 1001,
        TrackHasNoAvailbleSeats = 1002,
        EnrollmentNotFound = 2000,
        EnrollmentAlreadyCompleted = 2001,
        EnrollmentIsNotActive = 2002,
        EnrollmentPaymentCompleted = 2003,
        EnrollmentIsCancelled = 2004,
    }
}
