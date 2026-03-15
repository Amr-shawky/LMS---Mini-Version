namespace LMS___Mini_Version.Features.Shared
{
    public record RequestResponse<T>(
        T? Data,
        string Message = "",
        bool IsSuccess = true
    )
    {
        public static RequestResponse<T> Success(
            T data,
            string message = "Operation completed successfully"
        ) => new(data, message, true);

        public static RequestResponse<T> Fail(
            string message = "Operation failed"
        ) => new(default!, message, false);
    }
}
