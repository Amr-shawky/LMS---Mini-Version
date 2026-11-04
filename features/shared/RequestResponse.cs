namespace LMS___Mini_Version.features.shared
{
    public class RequestResponse
    {
        public bool Success { get; init; }
        public int StatusCode { get; init; }
        public IDictionary<string, string[]>? Errors { get; init; }

        public static RequestResponse OK(int statusCode = 200) => new()
         { StatusCode = statusCode, Success = true };

        public static RequestResponse Fail(int statusCode = 400,
            IDictionary<string, string[]>? errors = null) => new() {StatusCode = statusCode , Errors = errors };

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
