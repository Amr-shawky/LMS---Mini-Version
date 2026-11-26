namespace LMS___Mini_Version.features.InternFeature.AddIntern.Dtos
{
    public class CreatedInternDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int TrackId { get; set; }
        public string TrackName { get; set; } = string.Empty;
    }
}
