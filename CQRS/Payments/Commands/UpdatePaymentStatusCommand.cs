using LMS___Mini_Version.Domain.Enums;
using MediatR;

namespace LMS___Mini_Version.CQRS.Payments.Commands
{
    public record UpdatePaymentStatusCommand(int paymentId,PaymentStatus Status) : IRequest<RequestResult<bool>>;
   
}
