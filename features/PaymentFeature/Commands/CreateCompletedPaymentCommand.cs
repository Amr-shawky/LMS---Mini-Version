using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.PaymentFeature.Commands
{
    public record CreateCompletedPaymentCommand(int EnrollmentId, decimal TrackFees, PaymentMethod Method): IRequest<RequestResponse>
    {
    }
}
