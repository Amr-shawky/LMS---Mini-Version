using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackAvailability.Dtos;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.GetTrackAvailability.Queries
{
    public record GetTrackAvailabilityQuery(int TrackId) : IRequest<RequestResponse<GetTrackAvailabilityDto>>;   
}
