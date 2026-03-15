using System.Text.Json;

namespace LMS___Mini_Version.Errors.ApiResponse
{
    public class ApiExcptionResponse : ApiResponse
    {
        public string? Details { get; set; }

        public ApiExcptionResponse(int statusCode, string? message = null, string? details = null)
            : base(statusCode, message)

        {
            Details = details;
        }

        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

    }
}
