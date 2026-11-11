using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.features.InternFeature.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.Commands
{
    public record ApproveAndRegisterCommand(int internId, PaymentMethod PaymentMethod)
        : IRequest<RequestResponse<bool>>;
    
}
