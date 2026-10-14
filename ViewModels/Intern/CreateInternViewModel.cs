namespace LMS___Mini_Version.ViewModels.Intern
{
    /// <summary>
    /// [Trap 2 Fix] Input view model for creating an Intern.
    /// </summary>
    public class CreateInternViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int BirthYear { get; set; }
        public string Status { get; set; } = string.Empty;
        public int TrackId { get; set; }
    }
}
