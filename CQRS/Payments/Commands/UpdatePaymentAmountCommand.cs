using MediatR;

namespace LMS___Mini_Version.CQRS.Payments.Commands
{
    public record UpdatePaymentAmountCommand(int id,decimal NewAmount) : IRequest<RequestResult<bool>>;
    
}
