using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.DTO;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.Query
{
    public record CheckTrackAvaiabilityQuery (int trackId):IRequest<RequestResponse<CheckTrackAvailabilityDTO>>;
    
    
}
