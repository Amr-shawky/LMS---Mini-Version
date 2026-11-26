using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.features.InternFeature.ApproveAndRegisterIntern.Dtos;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.ApproveAndRegisterIntern.Commands
{
    public record ApproveAndRegisterInternCommand(int InternId, PaymentMethod? PaymentMethod)
        : IRequest<RequestResponse<InternRegistrationDto>>;
}
