using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LMS___Mini_Version.Feature.enrollmentFeature.endpoints
{
    public static class CancelEnrollmentEndpoint
    {
        public static void MapCancelEnrollmentEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/Enrollment/v2/{id:int}/cancel", async (int id, IMediator mediator) =>
            {
                var result = await mediator.Send(new cancelEnrollmentOrchestrator(id));
                if (!result.Success)
                {
                    return Results.BadRequest(EndpointResponse.Fail(result.Message, result.StatusCode, result.Errors));
                }

                return Results.Ok(EndpointResponse.Ok("Enrollment cancelled successfully"));
            }).WithTags("Enrollment V2")
            .WithSummary("Cancel an enrollment (v2)");
        }
    }
}
