using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Orchestrator
{
    public record CancelEnrollmentOrchestratorRequest(int enrollmentId):IRequest<RequestResult<bool>>;
   
}
