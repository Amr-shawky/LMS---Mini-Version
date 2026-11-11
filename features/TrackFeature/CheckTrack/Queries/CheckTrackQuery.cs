using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.CheckTrack.Dtos;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.CheckTrack.Queries
{
    public record CheckTrackQuery(int TrackId) : IRequest<RequestResponse<CheckTrackDto>>;
    
}
