namespace LMS___Mini_Version.ViewModels.InternViewModels
{
    public class CreateNewInternViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int BirthYear { get; set; }
        public string Status { get; set; } = string.Empty;
        public int TrackId { get; set; }
    }
}
