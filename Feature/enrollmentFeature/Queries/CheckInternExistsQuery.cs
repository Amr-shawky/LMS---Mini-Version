using exam_system.Features.Shared;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Queries
{
    public record CheckInternExistsQuery(int internId) : IRequest<RequestResponse<bool>>;
}
