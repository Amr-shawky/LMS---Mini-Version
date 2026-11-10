namespace LMS___Mini_Version.features.shared
{
    public enum ErrorCode
    {
        None,
        InternNotFound=1,
        InternAlreadyExists = 2,
        InternHasEnrollment = 3,
        TrackNotFound =1000,
        TrackIsNotActive = 1001,
        EnrollmentNotFound = 2000,
        EnrollmentAlreadyCompleted = 2001,
        EnrollmentIsNotActive = 2002,
        EnrollmentPaymentCompleted = 2003,
        EnrollmentIsCancelled = 2004,
    }
}
