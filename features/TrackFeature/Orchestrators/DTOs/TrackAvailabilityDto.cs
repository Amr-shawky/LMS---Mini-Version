namespace LMS___Mini_Version.features.TrackFeature.Orchestrators.DTOs
{
    public class TrackAvailabilityDto
    {
        public string TrackName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int MaxCapacity { get; set; }
        public int ReservedSeats { get; set; }
        public int AvailabeSeats { get; set; }
    }
}
