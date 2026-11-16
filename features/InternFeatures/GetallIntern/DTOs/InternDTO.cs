using LMS___Mini_Version.features.TrackFeature.GetAllTracks.Dtos;

namespace LMS___Mini_Version.features.InternFeatures.GetallIntern.DTOs;

public class InternDTO
{
    public int Id { get; set; }

    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Status { get; set; } = default!;

    public string TrackName { get; set; }

}
