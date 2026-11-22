namespace LMS___Mini_Version.features.shared
{
    public enum ErrorCode
    {
        None = 0,
        TrackNotFound = 1001,
        TrackNotActive = 1011,
        InvalidTrackId = 1111,
        TrackNotAvailable = 1008,
        TrackHasNoAvailableSeats = 1088,
        EmailAlreadyExists = 2001,
        InternNotCreated = 2011,
        InvalidInternId = 2003,
        InternNotFound = 2033,
        InternHasEnrollments = 2005,
        InternWithNotPendingStatus = 2008,
        InternHasActiveOrPendingEnrollments = 2088,
        InvalidFullName = 2009,
        InvalidEmail = 2010,
        InvalidBirthYear = 2011,
        EnrollmentNotFound = 3004,
        InvalidEnrollmentId = 3005,
        EnrollmentNotActive = 3044,
        EnrollmentAlreadyCompletedOrCancelled = 3444,
        PaymentNotFound = 4004,
        PaymentNotCompleted = 4044,


    }
}
