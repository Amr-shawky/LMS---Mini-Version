using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Infrastructure.DTO_S.EnrollmentDTO_s;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Orchestrator
{
    public record EnrollInternOrchestratorRequest
        (int internId,PaymentMethod PaymentMethod)
        
        : IRequest<RequestResult<EnrollmentDTO>> ;
   
}
