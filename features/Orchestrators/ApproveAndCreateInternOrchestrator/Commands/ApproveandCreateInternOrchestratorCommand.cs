using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.features.Orchestrators.ApproveAndCreateInternOrchestrator.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.Orchestrators.ApproveAndCreateInternOrchestrator.Commands
{
    public record ApproveandCreateInternOrchestratorCommand (int internId , PaymentMethod paymentMethod): IRequest<RequestResponse<ApproveAndRegisterDTO>>;
    
}
