using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.ApproveRegisterIntern
{
    public record ApproveRegisterInternOrchestrator(int InternId,PaymentMethod PaymentMethod) : IRequest<RequestResponse<ApproveRegisterInternDto>>;


}
