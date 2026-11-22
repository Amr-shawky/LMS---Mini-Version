using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeatures.ApproveIntern.Orchestrator
{
    public record ApproveInternOrchestrator(int InternId, PaymentMethod PaymentMethod) : IRequest<RequestResponse<int>>;
   
}
