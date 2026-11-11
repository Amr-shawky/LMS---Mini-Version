using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetActiveTracks.DTOs;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.GetActiveTracks.Queries
{
    public record GetActiveTracksQuery : IRequest<RequestResponse<IEnumerable<ActiveTracksDTO>>>;

}
