using exam_system.Features.Shared;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.endpoints
{
    public static class ListEnrollmentsEndpoint
    {
        public static void MapListEnrollmentsEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/Enrollment/v2", async (
                int pageIndex = 1, int pageSize = 10,
                IMediator mediator = null!) =>
            {
                var response = await mediator.Send(new GetAllEnrollmentsQuery(pageIndex, pageSize));

                if (!response.Success)
                    return Results.Json(EndpointResponse<PaginatedResult<EnrollmentDto>>.FromResult(response),statusCode: response.StatusCode);

                return Results.Ok(EndpointResponse<PaginatedResult<EnrollmentDto>>.Ok(response.Data!, response.Message));
            })
            .WithTags("Enrollment V2")
            .WithName("ListEnrollments");
        }
    }
}