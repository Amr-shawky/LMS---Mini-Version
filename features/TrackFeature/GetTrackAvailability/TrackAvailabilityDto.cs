namespace LMS___Mini_Version.features.TrackFeature.GetTrackAvailability
{
    public class TrackAvailabilityDto
    {
        
        public string TrackName { get; set; }
        public bool IsActive { get; set; }
        public int MaxCapacity { get; set; }
        public int ReservedSeats { get; set; }
        public int AvailableSeats { get; set; }
    }
}
