using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.Domain.Entities
{

    public class Enrollment : baseEntity
    {

        public int InternId { get; set; }
        public Intern Intern { get; set; } = null!;

        public int TrackId { get; set; }
        public Track Track { get; set; } = null!;

        public DateTime EnrollmentDate { get; set; }
        public EnrollmentStatus Status { get; set; }

        public Payment? Payment { get; set; }
    }
}
