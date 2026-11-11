using LMS___Mini_Version.Domain.Enums;
using System.Text.Json.Serialization;

namespace LMS___Mini_Version.features.EnrollmentFeatures.GetAllEnrollments.ViewModels
{
    public class EnrollmentsViewModel
    {
        public int EnrollmentId { get; set; }
        public int TrackId { get; set; }

        public string TrackName { get; set; } = string.Empty;

        public DateTime EnrollmentDate { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EnrollmentStatus EnrollmentStatus { get; set; }
    }
}
