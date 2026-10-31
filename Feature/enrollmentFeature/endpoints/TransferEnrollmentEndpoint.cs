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
            app.MapPost("api/Enrollment/v2/{id:int}/transfer/{newTrackId:int}",
                async (int id, int newTrackId, IMediator mediator) =>
                {
                    var result = await mediator.Send(new TransferEnrollmentOrchestrator(id, newTrackId));
                    if (!result.Success)
                        return Results.Json(
                            EndpointResponse.Fail(result.Message, result.StatusCode, result.Errors),
                            statusCode: result.StatusCode);

                    // Transfer is committed; refund processing is asynchronous via RabbitMQ.
                    return Results.Ok(EndpointResponse.Ok(result.Message));
                })
            .WithTags("Enrollment V2")
            .WithSummary("Transfer an enrollment to a new track (v2); refund (if applicable) is async");
        }
    }
}