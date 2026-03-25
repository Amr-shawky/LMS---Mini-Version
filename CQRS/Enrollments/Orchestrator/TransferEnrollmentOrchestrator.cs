using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Orchestrator
{
    public record TransferEnrollmentOrchestrator(int oldTrackId,int newTrackId):IRequest<RequestResult<bool>>;
   
}
