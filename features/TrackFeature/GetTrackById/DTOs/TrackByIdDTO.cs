namespace LMS___Mini_Version.features.TrackFeature.GetTrackById.DTOs
{
    public class TrackByIdDTO
    {
        public int Id { get; set; }
        public string TrackName { get; set; }   = string.Empty;
        public bool IsActive { get; set; }  
        public decimal Fees { get; set; }
    }
}
