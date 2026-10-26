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
    public static class GetEnrollmentByIdEndpoint
    {
        public static void MapGetEnrollmentByIdEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/Enrollment/v2/{id:int}", async (int id, IMediator mediator) =>
            {
                var response = await mediator.Send(new GetEnrollmentByIdQuery(id));
                if (!response.Success || response.Data == null)
                {
                    return Results.NotFound(EndpointResponse<EnrollmentViewModel>.Fail(response.Message, response.StatusCode, response.Errors));
                }

                return Results.Ok(EndpointResponse<EnrollmentViewModel>.Ok(response.Data.ToViewModel()));
            }).WithTags("Enrollment V2")
            .WithSummary("Get enrollment by ID (v2)");
        }
    }
}
