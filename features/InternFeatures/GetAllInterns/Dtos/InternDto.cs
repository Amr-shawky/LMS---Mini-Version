namespace LMS___Mini_Version.features.InternFeatures.GetAllInterns.Dtos
{
    public class InternDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        
        public string TrackName { get; set; } = string.Empty;
    }
}
