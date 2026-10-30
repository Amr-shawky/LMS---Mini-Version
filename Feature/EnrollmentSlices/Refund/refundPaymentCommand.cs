using exam_system.Features.Shared;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Commands;

// Data=0 means no eligible original completed payment; Data>0 is refunded PaymentId.
public record refundPaymentCommand(int EnrollmentID) : IRequest<RequestResponse<int>>;
