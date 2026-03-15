namespace LMS___Mini_Version.Features.Shared
{
    public record EndpointResponse<T>(
           T? Data,
           string Message = "",
           bool IsSuccess = true,
           int StatusCode = 200,
           List<string>? Errors = null,
           DateTime? Timestamp = null
       )
    {
        public List<string> Errors { get; init; } = Errors ?? new();
        public DateTime? Timestamp { get; init; } = Timestamp ?? DateTime.UtcNow;

        public static EndpointResponse<T> SuccessResponse(
            T data,
            string message = "Operation completed successfully",
            int statusCode = 200
        ) => new(data, message, true, statusCode);

        public static EndpointResponse<T> ErrorResponse(
            string message = "Operation failed",
            int statusCode = 400,
            List<string>? errors = null
        ) => new(default!, message, false, statusCode, errors ?? new() { message });

        public static EndpointResponse<T> ValidationErrorResponse(
            Dictionary<string, List<string>> validationErrors,
            string message = "Validation failed"
        )
        {
            var errors = new List<string>();
            foreach (var kvp in validationErrors)
                foreach (var err in kvp.Value)
                    errors.Add($"{kvp.Key}: {err}");

            return new(default!, message, false, 400, errors);
        }

        public static EndpointResponse<T> NotFoundResponse(
            string message = "Resource not found"
        ) => new(default!, message, false, 404, new() { message });

        public static EndpointResponse<T> UnauthorizedResponse(
            string message = "Unauthorized access"
        ) => new(default!, message, false, 401, new() { message });

        public static EndpointResponse<T> ForbiddenResponse(
            string message = "Access forbidden"
        ) => new(default!, message, false, 403, new() { message });

        public static EndpointResponse<T> ConflictResponse(
            string message = "Resource conflict"
        ) => new(default!, message, false, 409, new() { message });

        public static EndpointResponse<T> InternalServerErrorResponse(
            string message = "Internal server error occurred"
        ) => new(default!, message, false, 500, new() { message });
    }
}
