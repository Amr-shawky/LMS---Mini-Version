using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Enums;
using MediatR;

namespace LMS___Mini_Version.Feature.paymentFeature.Commands
{
    public record CreatePaymentCommand(int EnrollmentId, decimal Amount, PaymentMethod Method, PaymentStatus Status) : IRequest<RequestResponse<int>>;
}
