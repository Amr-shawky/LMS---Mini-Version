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
    public static class GetAllEnrollmentsEndpoint
    {
        public static void MapGetAllEnrollmentsEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/Enrollment/v2", async (IMediator mediator) =>
            {
                var response = await mediator.Send(new GetAllEnrollmentsQuery());
                if (!response.Success)
                {
                    return Results.BadRequest(EndpointResponse<IEnumerable<EnrollmentViewModel>>.Fail(response.Message, response.StatusCode, response.Errors));
                }

                var viewModels = response.Data!.Select(d => d.ToViewModel());
                return Results.Ok(EndpointResponse<IEnumerable<EnrollmentViewModel>>.Ok(viewModels));
            }).WithTags("Enrollment V2")
            .WithSummary("Get all enrollments (v2)");
        }
    }
}
