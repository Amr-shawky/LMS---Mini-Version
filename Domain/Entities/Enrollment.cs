using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.Domain.Entities
{
    public class Enrollment : BaseEntity<int>
    {
        public int TrackId { get; set; }
        public Track Track { get; set; }
        public int InternId { get; set; }
        public Intern Intern { get; set; }
        public string PaymentStatus { get; set; } = "Completed";
        public string EnrollmentStatus { get; set; } = "Active";
        
    }
}
