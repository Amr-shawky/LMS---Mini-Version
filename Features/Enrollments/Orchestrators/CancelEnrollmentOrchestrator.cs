using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Orchestrators
{
    public record CancelEnrollmentOrchestrator(int EnrollmentId):IRequest;
    
}
