namespace LMS___Mini_Version.DTOs
{
    /// <summary>
    /// Input DTO for the enrollment action — carries just the IDs needed.
    /// </summary>
    public class CreateEnrollmentDto
    {
        public int InternId { get; set; }
        public int TrackId { get; set; }
    }
}
