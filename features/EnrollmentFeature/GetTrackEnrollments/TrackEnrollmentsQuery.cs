using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.GetTrackEnrollments
{
    public record TrackEnrollmentsQuery(int trackId) : IRequest<RequestResponse<IEnumerable<TrackEnrollmentsDto>>>;
}
