namespace exam_system.Features.Shared;

public class EndpointResponse<T>
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public IDictionary<string, string[]>? Errors { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public EndpointResponse() { }

    public EndpointResponse(bool success, int statusCode, string message, T? data = default, IDictionary<string, string[]>? errors = null)
    {
        Success = success;
        StatusCode = statusCode;
        Message = message;
        Data = data;
        Errors = errors;
        Timestamp = DateTime.UtcNow;
    }

    public static EndpointResponse<T> Ok(T data, string message = "Success", int statusCode = 200)
        => new(true, statusCode, message, data);

    public static EndpointResponse<T> Created(T data, string message = "Created successfully")
        => new(true, 201, message, data);

    public static EndpointResponse<T> Fail(string message, int statusCode = 400, IDictionary<string, string[]>? errors = null)
        => new(false, statusCode, message, default, errors);

    public static EndpointResponse<T> FromResult(RequestResponse<T> result)
    {
        return new EndpointResponse<T>(
            result.Success,
            result.StatusCode,
            result.Message,
            result.Data,
            result.Errors
        );
    }
}

public class EndpointResponse : EndpointResponse<object>
{
    public static EndpointResponse Ok(string message = "Success", int statusCode = 200)
        => new() { Success = true, StatusCode = statusCode, Message = message };

    public static new EndpointResponse Fail(string message, int statusCode = 400, IDictionary<string, string[]>? errors = null)
        => new() { Success = false, StatusCode = statusCode, Message = message, Errors = errors };

    public static EndpointResponse FromResult(RequestResponse result)
    {
        return new EndpointResponse
        {
            Success = result.Success,
            StatusCode = result.StatusCode,
            Message = result.Message,
            Errors = result.Errors,
            Timestamp = DateTime.UtcNow
        };
    }
}
