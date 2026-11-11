namespace LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.DTOs
{
    public class CheckTrackAvaiabilityDto
    {
        public string TrackName { get; set; }  
        public int MaxCapacity { get; set; }
        public bool IsActive { get; set; }
        public int ReservedSeats { get; set; }
        public int AvailableSeats { get; set; }
    }
}
