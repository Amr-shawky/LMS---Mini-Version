using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.PaymentFeature.CreatePayment.Commands
{
    public record CreatePaymentCommand(int EnrollmentId,decimal Amount,PaymentMethod Method) : IRequest<RequestResponse<int>>;
}
