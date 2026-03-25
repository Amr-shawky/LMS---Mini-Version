using LMS___Mini_Version.Infrastructure.DTO_S.EnrollmentDTO_s;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Orchestrator
{
    public record EnrollInternOrchestratorRequest
        (int internId)
        
        : IRequest<RequestResult<EnrollmentDTO>> ;
   
}
