namespace LMS___Mini_Version.Errors.ApiResponse
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public required IEnumerable<string> Errors { get; set; }

        public ApiValidationErrorResponse(string? message = null)
            : base(400, message)
        {

        }

    }
}
