using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LMS___Mini_Version.Feature.enrollmentFeature.endpoints
{
    public static class TransferEnrollmentEndpoint
    {
        public static void MapTransferEnrollmentEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/Enrollment/v2/{id:int}/transfer/{newTrackId:int}", async (int id, int newTrackId, IMediator mediator) =>
            {
                var result = await mediator.Send(new TransferEnrollmentOrchestrator(id, newTrackId));
                if (!result.Success)
                {
                    return Results.BadRequest(EndpointResponse.Fail(result.Message, result.StatusCode, result.Errors));
                }

                return Results.Ok(EndpointResponse.Ok("Enrollment transferred successfully"));
            }).WithTags("Enrollment V2")
            .WithSummary("Transfer an enrollment (v2)");
        }
    }
}
