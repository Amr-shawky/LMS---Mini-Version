namespace LMS___Mini_Version.features.InternFeature.CreateIntern.ViewModels
{
    public class CreateInternInputViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int BirthYear { get; set; }
        public int TrackId { get; set; }
    }
}
