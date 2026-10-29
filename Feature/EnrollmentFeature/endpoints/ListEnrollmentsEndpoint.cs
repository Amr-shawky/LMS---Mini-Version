using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;

namespace LMS___Mini_Version.Feature.EnrollmentFeature.endpoints
{
    public static class ListEnrollmentsEndpoint
    {
        public static void MapListEnrollmentsEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/Enrollment/v2", async(IMediator _mediator, int pageIndex = 1, int pageSize = 10) =>
            {
                var response = await _mediator.Send(new GetAllEnrollmentsQuery(pageIndex, pageSize));

                if (!response.Success || response.Data == null)
                {
                    return Results.Json( EndpointResponse<PaginatedResult<EnrollmentViewModel>>.Fail(
                            response.Message, response.StatusCode, response.Errors),
                        statusCode: response.StatusCode);
                }

                // Map the DTOs to ViewModels
                var viewModels = response.Data.Items.Select(d=>d.ToViewModel()).ToList();

                // Create a new PaginatedResult with the ViewModels
                var paginated = PaginatedResult<EnrollmentViewModel>.Create(
                    viewModels, response.Data.TotalCount, response.Data.PageIndex, response.Data.PageSize);
                
                return Results.Ok(EndpointResponse<PaginatedResult<EnrollmentViewModel>>.Ok(paginated));
            
            }).WithTags("Enrollment V2")
            .WithSummary("List enrollments paginated (v2)");
        }
    }
}
