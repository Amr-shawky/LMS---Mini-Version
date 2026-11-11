using LMS___Mini_Version.features.EnrollmentFeature.GetEnrollmentsByTrack.Dtos;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.GetEnrollmentsByTrack.Query
{
    public record GetEnrollmentsByTrackQuery(int TrackId)
        : IRequest<RequestResponse<IEnumerable<EnrollmentByTrackDto>>>;
}
