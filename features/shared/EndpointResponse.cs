namespace LMS___Mini_Version.features.shared
{
    public class EndpointResponse
    {
        public bool Success { get; init; }
        public int StatusCode { get; init; }
        public string ErrorCode { get; init; } = string.Empty;
        public string? Message { get; init; } = "";

        public DateTime TimeStamp { get; init; } = DateTime.UtcNow;
        public static EndpointResponse OK(int statusCode = 200, string msg = "") => new()
        { StatusCode = statusCode, Success = true, Message = msg };

        public static EndpointResponse Fail(int statusCode = 400, string msg = "",
            string errorCode = "None") => new() { StatusCode = statusCode, ErrorCode = errorCode, Message = msg };

    }

    public class EndpointResponse<T> : EndpointResponse
    {
        public T? Data { get; init; }
        public static EndpointResponse<T> OK(T data, int statusCode = 200,string msg = "") => new()
        { StatusCode = statusCode, Success = true, Message = msg , Data = data};

        public static new EndpointResponse<T> Fail(int statusCode = 400, string msg = "",
            string errorCode = "None") => new() { StatusCode = statusCode, ErrorCode = errorCode, Message = msg };

    }
}
