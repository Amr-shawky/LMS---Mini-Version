namespace LMS___Mini_Version.features.TrackFeature.TrackAvaillability.Dtos
{
    public record TrackAvailabilityResponseDto
    (
        string TrackName,
        bool IsActive,
        int MaxCapacity,
        int ReservedSeats,
        int AvailableSeats
    );
}
