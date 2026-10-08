using exam_system.Features.Shared;
using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Feature.paymentFeature.Queries
{
    public record GetAllPaymentsQuery : IRequest<RequestResponse<IEnumerable<PaymentDto>>>;
}
