using LMS___Mini_Version.Features.Shared;
using MediatR;

namespace LMS___Mini_Version.Features.TrackFeature.Queries
{
    public record GetActiveEnrollmentCountByTrackQuery(int Id) : IRequest<RequestResponse<int>>;

}
