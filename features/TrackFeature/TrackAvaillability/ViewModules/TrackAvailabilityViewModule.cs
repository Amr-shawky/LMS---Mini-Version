namespace LMS___Mini_Version.features.TrackFeature.TrackAvaillability.ViewModules
{
    public class TrackAvailabilityViewModule
    {
        public string TrackName { get; set; } = default!;
        public bool IsActive { get; set; }
        public int MaxCapacity { get; set; }
        public int ReservedSeats { get; set; }
        public int AvailableSeats { get; set; }
    }
}
