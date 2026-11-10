namespace LMS___Mini_Version.features.TrackFeature.GetTrackByAvailability.DTOS
{
    public record GetTrackByAvailabilityDTO(string TrackName,
        bool IsActive,
        int ReservedSeats,
        int MaxCapacity,
        int AvailableSeats);
}
