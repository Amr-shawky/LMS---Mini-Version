using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollmentHistory.Dtos
{
    public class EnrollmentDto
    {
        public int EnrollmentId { get; set; }
        public int TrackId { get; set; }
        public string TrackName { get; set; } = null!;
        public DateTime EnrollmentDate { get; set; }
        public EnrollmentStatus EnrollmentStatus { get; set; }
    }
}