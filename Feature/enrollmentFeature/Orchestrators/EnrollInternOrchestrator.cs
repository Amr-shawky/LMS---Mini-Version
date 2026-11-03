using exam_system.Features.Shared;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators
{
    public record EnrollInternOrchestrator(int InternId, int TrackId) : IRequest<RequestResponse<int>>;
}
