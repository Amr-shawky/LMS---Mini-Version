using exam_system.Features.Shared;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Commands
{
    public record CancelEnrollmentCommand(int InternId, int TrackId) : IRequest<RequestResponse<bool>>;
}