using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackEnrollments.Dtos;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.GetTrackEnrollments.Queries
{
    public record GetTrackEnrollmentsQuery(int TrackId):IRequest<RequestResponse<IEnumerable<GetTrackEnrollmentsDto>>>;

}
