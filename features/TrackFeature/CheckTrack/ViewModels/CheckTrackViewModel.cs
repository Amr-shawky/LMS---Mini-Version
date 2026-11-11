namespace LMS___Mini_Version.features.TrackFeature.CheckTrack.ViewModels
{
    public class CheckTrackViewModel
    {
        public string TrackName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int MaxCapacity { get; set; }
        public int ReservedSeats { get; set; }
        public int AvailableSeats { get; set; }
    }
}
