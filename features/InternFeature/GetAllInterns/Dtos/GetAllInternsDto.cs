namespace LMS___Mini_Version.features.InternFeature.GetAllInterns.Dtos
{
    public class GetAllInternsDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }= string.Empty;
        public string Email { get; set; }= string.Empty;
        public string TrackName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
