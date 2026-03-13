namespace LMS___Mini_Version.ViewModels.Intern
{
    /// <summary>
    /// [Trap 2 Fix] Lightweight view model for intern list endpoints.
    /// </summary>
    public class InternSummaryViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string TrackName { get; set; } = string.Empty;
    }
}
