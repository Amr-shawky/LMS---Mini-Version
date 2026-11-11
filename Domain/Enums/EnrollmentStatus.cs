using System.Text.Json.Serialization;

namespace LMS___Mini_Version.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EnrollmentStatus
    {
        Pending,
        Active,
        Completed,
        Cancelled
    }
}
