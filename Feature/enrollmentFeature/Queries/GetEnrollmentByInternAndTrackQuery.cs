using exam_system.Features.Shared;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Queries
{
    public record GetEnrollmentByInternAndTrackQuery(int internId, int trackId) : IRequest<RequestResponse<bool>>;
}
