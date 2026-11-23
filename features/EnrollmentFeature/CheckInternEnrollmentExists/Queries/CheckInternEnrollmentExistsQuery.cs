using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.CheckInternEnrollmentExists.Queries
{
    public record CheckInternEnrollmentExistsQuery(int InternId, int TrackId) : IRequest<RequestResponse<bool>>;
}
