using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.shared.Behaviors;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.ApproveAndRegisterIntern.Orchestrators
{
    public record ApproveAndRegisterInternOrchestrator(int InternId,PaymentMethod Method) : IRequest<RequestResponse<int>>, ITransactionalRequest;
    
}
