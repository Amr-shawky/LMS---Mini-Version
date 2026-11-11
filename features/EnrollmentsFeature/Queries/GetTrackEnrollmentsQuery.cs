using LMS___Mini_Version.features.EnrollmentsFeature.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentsFeature.Queries
{
    public record GetTrackEnrollmentsQuery(int trackId) : IRequest<RequestResponse<IEnumerable<TrackEnrollmentDTO>>>;
     
}
