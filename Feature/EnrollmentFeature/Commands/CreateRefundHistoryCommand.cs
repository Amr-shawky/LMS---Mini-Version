using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Enums;
using MediatR;

namespace LMS___Mini_Version.Feature.EnrollmentFeature.Commands
{
    public record CreateRefundHistoryCommand(
        int PaymentId,
        int EnrollmentId,
        PaymentStatus PreviousStatus,
        PaymentStatus NewStatus) : IRequest<RequestResponse>;
}
