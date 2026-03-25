using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Infrastructure.DTO_S.PayementsDTO_s;
using MediatR;

namespace LMS___Mini_Version.CQRS.Payments.Commands
{
    public record StageNewPaymentCommand(int EnrollmentId,
        decimal Amount,
        PaymentMethod Method)
        : IRequest<RequestResult<PaymentDTO>>;
   
}
