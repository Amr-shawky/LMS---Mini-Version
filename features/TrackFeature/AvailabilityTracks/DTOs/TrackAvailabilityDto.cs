namespace LMS___Mini_Version.features.TrackFeature.AvailabilityTracks.DTOs;

public record TrackAvailabilityDto(
    string TrackName,
    bool IsActive,
    int MaxCapacity,
    int ReservedSeats,
    int AvailableSeats);