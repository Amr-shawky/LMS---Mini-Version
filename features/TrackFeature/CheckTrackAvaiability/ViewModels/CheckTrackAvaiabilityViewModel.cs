namespace LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.ViewModels
{
    public class CheckTrackAvaiabilityViewModel
    {
        public string TrackName { get; set; }  
        public int MaxCapacity { get; set; }
        public int ReservedSeats { get; set; }
        public int AvailableSeats { get; set; }
        public bool IsActive { get; set; }
    }
}
