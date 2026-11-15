using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.PaymentFeature.CreatePayment
{
    public record CreatePaymentCommand(int enrollmentId, PaymentMethod paymentMethod) : IRequest<RequestResponse>;

}
