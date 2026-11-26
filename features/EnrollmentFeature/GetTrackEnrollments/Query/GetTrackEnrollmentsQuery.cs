using LMS___Mini_Version.features.EnrollmentFeature.GetTrackEnrollments.Dtos;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.GetTrackEnrollments.Query
{
    public record GetTrackEnrollmentsQuery(int TrackId)
        : IRequest<RequestResponse<IEnumerable<TrackEnrollmentDto>>>;
}
