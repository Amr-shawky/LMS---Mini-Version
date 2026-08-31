namespace LMS___Mini_Version.ViewModels.Intern
{
    /// <summary>
    /// [Trap 2 Fix] Detailed view model for single-intern endpoints.
    /// </summary>
    public class InternDetailViewModel
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
