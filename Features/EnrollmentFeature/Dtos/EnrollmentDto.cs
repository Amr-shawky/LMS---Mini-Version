using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.Features.EnrollmentFeature.Dtos
{
    public class EnrollmentDto
    {
        public int TrackId { get; set; }
        public int InternId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public EnrollmentStatus Status { get; set; }
    }
}
