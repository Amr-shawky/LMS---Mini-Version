using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Features.EnrollmentFeature.Dtos;

namespace LMS___Mini_Version.Features.InternsFeature.Dtos
{
    public class InternDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int BirthYear { get; set; }
        public int CountEnrollment { get; set; }

        public string TrackName { get; set; }
        public List<Enrollment> Enrollments { get; set; } = new();

        //public List<DateTime> EnrollmentDate { get; set; } = new();




    }
}
