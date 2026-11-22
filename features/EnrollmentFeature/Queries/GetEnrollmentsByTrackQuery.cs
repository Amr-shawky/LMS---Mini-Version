using LMS___Mini_Version.features.EnrollmentFeature.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.Queries
{
    public record GetEnrollmentsByTrackQuery(int TrackId): IRequest<RequestResponse<IEnumerable<TrackEnrollmentsDto>>>
    {
    }
}
