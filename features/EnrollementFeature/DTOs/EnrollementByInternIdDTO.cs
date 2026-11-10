using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.features.EnrollementFeature.DTOs
{
    public class EnrollementByInternIdDTO
    {
        public int EnrollmentId { get; set; }
        public int TrackId { get; set; }
        public string TrackName { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public EnrollmentStatus Status { get; set; }
    }
}
