using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.endpoints
{
    public static class CancelEnrollmentEndpoint
    {
        public static void MapCancelEnrollmentEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/Enrollment/v2/{id:int}/cancel", async (int id, IMediator mediator) =>
            {
                var response = await mediator.Send(new cancelEnrollmentOrchestrator(id));

                if (!response.Success)
                    return Results.Json(
                        EndpointResponse.FromResult(response),
                        statusCode: response.StatusCode);

                return Results.Ok(EndpointResponse.Ok(response.Message));
            })
                .WithTags("Enrollment V2")
                .WithName("CancelEnrollment");
        }
    }
}
