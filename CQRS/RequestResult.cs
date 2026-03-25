using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.CQRS
{
    public record RequestResult<TResult>
    {
        public TResult? Data { get; init; }
        public bool IsSuccess { get; init; }
        public ErrorCode ErrorCode { get; init; }
       

        // Success
        public static RequestResult<TResult> Success(TResult data) =>
            new RequestResult<TResult>
            {
                Data = data,
                IsSuccess = true,
                ErrorCode = ErrorCode.None,
             
            };

        // Failure
        public static RequestResult<TResult> Failure(ErrorCode errorCode) =>
            new RequestResult<TResult>
            {
                Data = default,
                IsSuccess = false,
                ErrorCode = errorCode,
              
            };
    }
}
