using exam_system.Features.Shared;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Commands
{
    /// <summary>
    /// Attempts to refund the payment linked to the given enrollment.
    ///
    /// Returns the PaymentId when a refund was applied successfully.
    /// Returns 0 when no eligible payment was found (missing, Pending, Failed, or already Refunded).
    ///
    /// Returning the PaymentId lets callers pass it directly to CreateRefundHistoryCommand
    /// without needing a second repository query.
    /// </summary>
    public record refundPaymentCommand(int EnrollmentID) : IRequest<RequestResponse<int>>;
}