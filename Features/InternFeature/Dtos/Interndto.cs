using LMS___Mini_Version.Domain.Entities;

namespace LMS___Mini_Version.Features.InternFeature.Dtos
{
    public class Interndto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string TrackName { get; set; } = string.Empty;
        public IEnumerable<DateTime> EnrollmentDate { get; set; }
        public int EnrollmentCount { get; set; }
        public List<Enrollment> Enrollments { get; set; } = new();
    }
}
