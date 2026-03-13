using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.ViewModels.Enrollment
{
    /// <summary>
    /// [Trap 2 Fix] Output view model for enrollment responses.
    /// </summary>
    public class EnrollmentViewModel
    {
        public int Id { get; set; }
        public string InternName { get; set; } = string.Empty;
        public string TrackName { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
