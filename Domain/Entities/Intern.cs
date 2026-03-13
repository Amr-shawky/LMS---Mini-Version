namespace LMS___Mini_Version.Domain.Entities
{
    public class Intern
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int BirthYear { get; set; }
        public string Status { get; set; } = string.Empty;

        public int TrackId { get; set; }
        public Track Track { get; set; } = null!;

        public List<Enrollment> Enrollments { get; set; } = new();
    }
}