namespace LMS___Mini_Version.Features.Shared
{
    public class RequestResponse
    {
        public int StatusCode { get; init; }
        public bool Success { get; init; }
        public IDictionary<string, string[]>?  Errors { get; init; }
        public static RequestResponse OK(int statusCode) => new()
        { Success = true, StatusCode = statusCode };

        public static RequestResponse Fail(int statusCode
            , IDictionary<string, string[]> errors) => new()
        { StatusCode = statusCode , Errors = errors };
    }
    public class RequestResponse<T> :RequestResponse
    {

        public T? Data { get; init; }

        public static RequestResponse<T> OK(int statusCode , T data) => new()
        { Success = true, StatusCode = statusCode , Data = data};

        public static new RequestResponse<T> Fail(int statusCode
            , IDictionary<string, string[]> errors) => new()
            { StatusCode = statusCode, Errors = errors };

    }
}
