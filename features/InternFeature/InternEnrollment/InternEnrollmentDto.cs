using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.features.InternFeature.InternEnrollment
{
    public class InternEnrollmentDto
    {
        
        public int EnrollmentId { get; set; }
        public int TrackId { get; set; }
        public string TrackName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public EnrollmentStatus EnrollmentStatus { get; set; }
    }
}
