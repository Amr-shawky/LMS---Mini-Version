using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.ApproveAndRegister.Commands
{
    public record ApproveAndRegisterCommand(
       int InternId,
       PaymentMethod Method
   ) : IRequest<RequestResponse<int>>;
}
