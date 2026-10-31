using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Enums;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Commands
{
    /// <summary>
    /// Creates a RefundHistory record that documents the refund transition.
    ///
    /// PaymentId is passed explicitly so this handler only needs one repository (R8 compliance).
    /// The caller (cancel orchestrator or refund consumer) already has the payment context
    /// or can pass 0 when it does not have the PaymentId -- the handler will skip history
    /// if PaymentId is 0 (defensive programming).
    ///
    /// This command is called AFTER refundPaymentCommand confirms a refund was applied.
    /// Both commands must succeed atomically (R24).
    /// </summary>
    public record CreateRefundHistoryCommand(
        int EnrollmentId,
        int PaymentId,
        PaymentStatus PreviousStatus,
        PaymentStatus NewStatus) : IRequest<RequestResponse<bool>>;
}