using MediatR;
using exam_system.Features.Shared;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Commands
{
    public record refundPaymentCommand (int EnrollmentID) : IRequest<RequestResponse<bool>>;

}
