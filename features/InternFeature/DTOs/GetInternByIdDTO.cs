namespace LMS___Mini_Version.features.InternFeature.DTOs
{
    public class GetInternByIdDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Status {  get; set; } = string.Empty; 
        public int TrackId { get; set; }    
    } 
}
