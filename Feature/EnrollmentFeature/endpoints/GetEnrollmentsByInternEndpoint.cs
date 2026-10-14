using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LMS___Mini_Version.Feature.enrollmentFeature.endpoints
{
    public static class GetEnrollmentsByInternEndpoint
    {
        public static void MapGetEnrollmentsByInternEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/Enrollment/v2/intern/{internId:int}", async (int internId, IMediator mediator, int pageIndex = 1, int pageSize = 10) =>
            {
                var response = await mediator.Send(new GetEnrollmentsByInternQuery(internId, pageIndex, pageSize));
                if (!response.Success || response.Data == null)
                {
                    return Results.BadRequest(EndpointResponse<PaginatedResult<EnrollmentViewModel>>.Fail(response.Message, response.StatusCode, response.Errors));
                }

                var viewModels = response.Data.Items.Select(d => d.ToViewModel()).ToList();
                var paginated = PaginatedResult<EnrollmentViewModel>.Create(viewModels, response.Data.TotalCount, response.Data.PageIndex, response.Data.PageSize);
                return Results.Ok(EndpointResponse<PaginatedResult<EnrollmentViewModel>>.Ok(paginated));
            }).WithTags("Enrollment V2")
            .WithSummary("Get enrollments by intern ID paginated (v2)");
        }
    }
}
