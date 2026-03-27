using LMS___Mini_Version.DTOs;

namespace LMS___Mini_Version.CQRS.RequestResult
{
    public record RequestResult<TResult>(TResult Result, bool IsSuccess, ErrorCode ErrorCode )
    {
        public static RequestResult<TResult> Success(TResult result) => new(result, true, ErrorCode.None);
        public static RequestResult<TResult> Failure(ErrorCode errorCode) => new(default!, false, errorCode);

        internal static RequestResult<TrackDto> Success(IQueryable<TrackDto> trackdto)
        {
            throw new NotImplementedException();
        }
    }
}
