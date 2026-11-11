using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.TrackAvaillability.Dtos;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.TrackAvaillability.Query
{
    public record GetTrackAvailabilityQuery(int TrackId) : IRequest<RequestResponse<TrackAvailabilityResponseDto>>;
}
