using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackByAvailability.DTOS;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.GetTrackByAvailability.Query
{
    public record GetTrackByAvailabilityQuery(int TrackId) : IRequest<RequestResponse<GetTrackByAvailabilityDTO>>;
}
