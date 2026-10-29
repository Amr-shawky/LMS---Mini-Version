using exam_system.Features.Shared;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Commands
{
    public record UpdateEnrollmentTrackCommand(int EnrollmentId, int NewTrackId) : IRequest<RequestResponse>;
    
}
