using exam_system.Features.Shared;
using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Feature.paymentFeature.Queries
{
    public record GetAllPaymentsQuery(int PageIndex = 1, int PageSize = 10) : IRequest<RequestResponse<PaginatedResult<PaymentDto>>>;
}
