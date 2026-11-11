using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.features.EnrollmentFeatures.GetEnrollments.Dtos
{
    public class EnrollmentDto
    {
        public int EnrollmentId { get; set; }
        public int TrackId { get; set; }

        public int InternId { get; set; }
        public string TrackName { get; set; } = string.Empty;

        public DateTime EnrollmentDate { get; set; }
        public EnrollmentStatus EnrollmentStatus { get; set; }
    }
}
