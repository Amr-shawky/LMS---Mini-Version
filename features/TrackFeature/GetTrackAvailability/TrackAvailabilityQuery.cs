using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.GetTrackAvailability
{
    public record TrackAvailabilityQuery(int trackId) : IRequest<RequestResponse<TrackAvailabilityDto>>;
    
}
