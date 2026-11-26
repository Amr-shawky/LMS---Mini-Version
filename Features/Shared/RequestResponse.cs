namespace LMS___Mini_Version.Features.Shared
{
    public class RequestResponse
    {
        public bool Success { get; init; }
        public int StatusCode { get; init; }
        public IDictionary<string, string[]>? Errors { get; init; }
        public string? Message { get; init; }

        public static RequestResponse OK(string message = null, int statusCode = 200) => new()
        { StatusCode = statusCode,  Success = true ,Message = message};

        public static RequestResponse Fail(int statusCode = 400,
            IDictionary<string, string[]>? errors = null,string message = null) => new() { StatusCode = statusCode, Errors = errors ,Message = message};

    }
    public class RequestResponse<T> : RequestResponse
    {
        public T? Data { get; init; }

        public static RequestResponse<T> OK(T data, int statusCode = 200)
            => new() { Data = data, StatusCode = statusCode, Success = true };

        public static new RequestResponse<T> Fail(int statusCode = 400,
            IDictionary<string, string[]>? errors = null) => new() { StatusCode = statusCode, Errors = errors };
    }
}
