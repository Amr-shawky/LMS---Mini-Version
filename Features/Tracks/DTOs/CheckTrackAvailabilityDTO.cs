namespace LMS___Mini_Version.Features.Tracks.DTOs
{
    public class CheckTrackAvailabilityDTO
    {
        public string TrackName { get; set; }
        public bool IsActive { get; set; }
        public int MaxCapacity { get; set; }
        public int ReservedSeats { get; set; }
        public int AvailableSeats { get; set; }
    }
}
