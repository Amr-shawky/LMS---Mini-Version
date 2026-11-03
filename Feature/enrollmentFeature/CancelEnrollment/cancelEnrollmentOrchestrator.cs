using exam_system.Features.Shared;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.CancelEnrollment
{
    public record cancelEnrollmentOrchestrator(int enrollmentID) : IRequest<RequestResponse>;
}
