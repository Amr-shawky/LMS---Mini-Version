using LMS___Mini_Version.features.EnrollmentFeature.GetEnrollmentTrackIntern.DTOS;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.GetEnrollmentTrackIntern.Quey
{
    public record GetEnrollmentTrackInternQuery(int TrackId) : IRequest<RequestResponse<IEnumerable<GetEnrollmentTrackInternDTO>>>;
}
