namespace LMS___Mini_Version.DTOs
{
    /// <summary>
    /// [Trap 2 Fix] Data Transfer Object for Intern.
    /// </summary>
    public class InternDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int BirthYear { get; set; }
        public string Status { get; set; } = string.Empty;
        public int TrackId { get; set; }
        public string TrackName { get; set; } = string.Empty;
    }
}
