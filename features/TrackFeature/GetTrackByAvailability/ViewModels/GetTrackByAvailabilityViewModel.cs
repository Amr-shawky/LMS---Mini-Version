namespace LMS___Mini_Version.features.TrackFeature.GetTrackByAvailability.ViewModels
{
    public record GetTrackByAvailabilityViewModel(string TrackName, bool IsActive, int ReservedSeats, int MaxCapacity, int AvailableSeats);
}
