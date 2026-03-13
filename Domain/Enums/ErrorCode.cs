namespace LMS___Mini_Version.Domain.Enums
{
    public enum ErrorCode
    {
        // adding error code for all domain exceptions

        NoError = 0,

        /// intern errors
        /// 
        InternNotFound = 101,
        InvalidInternData = 102,

        /// track errors
        TrackNotFound = 201,
        InvalidTrackData = 202,
        TrackInactive = 203,
        TrackAtMaxCapacity = 204,

        /// payment errors
        
        PaymentFailed = 301,
        InvalidPaymentData = 302,
        PaymentNotFound = 303,

       /// Enrollement errors
       /// 

        EnrollmentNotFound = 401,
        InvalidEnrollmentData = 402,
        EnrollmentAlreadyExists = 403,
        EnrollmentCancelled = 404

    }
}
