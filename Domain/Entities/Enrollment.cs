using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.Domain.Entities
{
    /// <summary>
    /// Resolves the Many-to-Many relationship between Intern and Track.
    /// Each enrollment represents a single intern joining a single track.
    /// </summary>
    public class Enrollment
    {
        public int Id { get; set; }

        public int InternId { get; set; }
        public Intern Intern { get; set; } = null!;

        public int TrackId { get; set; }
        public Track Track { get; set; } = null!;

        public DateTime EnrollmentDate { get; set; }
        public EnrollmentStatus Status { get; set; }

        // Navigation: one enrollment can have one payment
        public Payment? Payment { get; set; }
    }
}
