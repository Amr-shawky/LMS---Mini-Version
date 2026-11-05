namespace LMS___Mini_Version.Features.Shared
{
    public class EndpointResponse
    {
        public int StatusCode { get; init; }
        public bool Success { get; init; }
        public string? Message { get; init; }
        public DateTime dateTime { get; init; } = DateTime.UtcNow;
        public IDictionary<string, string[]>? Errors { get; init; }

        public static EndpointResponse OK(int statusCode, string? message) => new()
        { Success = true, StatusCode = statusCode, Message = message };

        public static EndpointResponse Fail(int statusCode
            , IDictionary<string, string[]> errors, string? message) => new()
            { StatusCode = statusCode, Errors = errors, Message = message };
    }
    public class EndpointResponse<T> : EndpointResponse
    {
        public T? Data { get; init; }

        public static EndpointResponse<T> OK(int statusCode, T data,
            string? message ) => new()
        { Success = true, StatusCode = statusCode, Data = data  , Message = message };

        public static new EndpointResponse<T> Fail(int statusCode
            , IDictionary<string, string[]> errors , string? message) => new()
            { StatusCode = statusCode, Errors = errors , Message = message};
    }
}
