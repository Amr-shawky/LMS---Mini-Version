using exam_system.Features.Shared;
using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Queries
{
    public record GetAllEnrollmentsQuery(int PageIndex = 1, int PageSize = 10) : IRequest<RequestResponse<PaginatedResult<EnrollmentDto>>>
    {
        private const int MaxPageSize = 100;
        public int PageIndex { get; } = PageIndex < 1 ? 1 : PageIndex;
        public int PageSize { get; } = PageSize < 1 ? 10 : PageSize > MaxPageSize ? MaxPageSize : PageSize;
    }
}
