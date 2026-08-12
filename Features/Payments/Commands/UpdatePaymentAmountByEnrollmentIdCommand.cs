using MediatR;

namespace LMS___Mini_Version.Features.Payments.Commands
{
    public record UpdatePaymentAmountByEnrollmentIdCommand(int enrollmentId, decimal newAmount) : IRequest;
    
   
}
