namespace LMS___Mini_Version.Domain.Enums
{
    public enum ErrorCode
    {
        None = 0,

        InternNotFound=101,

        TrackNotFound=201,
        TrackNotActive=202,
        TrackFull=203,
        NoFees=204,


        EnrollMentNotExist=301,
        UpdateError=302,


        paymentNotExist=401,
    }
}
