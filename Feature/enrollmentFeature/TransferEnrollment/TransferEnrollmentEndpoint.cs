using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LMS___Mini_Version.Feature.enrollmentFeature.TransferEnrollment
{
    public static class TransferEnrollmentEndpoint
    {
        public static void MapTransferEnrollmentEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/Enrollment/v2/{id:int}/transfer/{newTrackId:int}", async (int id, int newTrackId, IMediator mediator) =>
            {
                var response = await mediator.Send(new TransferEnrollmentOrchestrator(id, newTrackId));
                if (!response.Success)
                {
                    return Results.BadRequest(EndpointResponse.Fail(response.Message, response.StatusCode, response.Errors));
                }

                return Results.Ok(EndpointResponse.Ok("Enrollment transferred successfully"));
            }).WithTags("Enrollment V2")
            .WithSummary("Transfer an enrollment to a new track (v2)");
        }
    }
}

