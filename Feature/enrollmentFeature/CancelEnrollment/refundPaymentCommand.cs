using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Enums;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.CancelEnrollment
{
    public class RefundResult
    {
        public bool IsEligible { get; set; }
        public int PaymentId { get; set; }
        public PaymentStatus PreviousStatus { get; set; }
    }

    public record refundPaymentCommand(int EnrollmentID) : IRequest<RequestResponse<RefundResult>>;
}
