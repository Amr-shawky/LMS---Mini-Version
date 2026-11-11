namespace LMS___Mini_Version.features.shared
{
    public class EndpointResponse
    {
        public bool Success { get; init; }
        public int StatusCode { get; init; }
        public IDictionary<string, string[]>? Errors { get; init; }
        public string? Message { get; init; } = "";
        public DateTime TimeStamp { get; init; } = DateTime.UtcNow;
        public ErrorCode ErrorCode { 
            get; set; } = ErrorCode.None;
        public static EndpointResponse OK(int statusCode = 200, string msg = "") => new()
        { StatusCode = statusCode, Success = true, Message = msg };

        public static EndpointResponse Fail(int statusCode = 400, string msg = "",
            IDictionary<string, string[]>? errors = null,ErrorCode errorCode = ErrorCode.None) => new()
            { StatusCode = statusCode, Errors = errors, Message = msg ,ErrorCode= errorCode};

    }

    public class EndpointResponse<T> : EndpointResponse
    {
        public T? Data { get; init; }
        public static EndpointResponse<T> OK(T data, int statusCode = 200,string msg = "") => new()
        { StatusCode = statusCode, Success = true, Message = msg , Data = data};

        public static new EndpointResponse<T> Fail(int statusCode = 400, string msg = "",
            IDictionary<string, string[]>? errors = null,ErrorCode errorCode= ErrorCode.None) => new() 
            { StatusCode = statusCode, Errors = errors, Message = msg ,ErrorCode = errorCode };

    }
}
