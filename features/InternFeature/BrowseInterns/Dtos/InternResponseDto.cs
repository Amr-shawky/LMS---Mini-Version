namespace LMS___Mini_Version.features.InternFeature.BrowseInterns.Dtos
{
    public record InternResponseDto(
        int Id,
        string FullName,
        string Email,
        string Status,
        string TrackName
    );
}
