using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.DTOs;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.Queries
{
    public record CheckTrackAvaiabilityQuery(int trackId) : IRequest<RequestResponse<CheckTrackAvaiabilityDto>>;
    
}
