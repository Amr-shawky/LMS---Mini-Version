using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.endpoints
{
    public static class TransferEnrollmentEndpoint
    {
        public static void MapTransferEnrollmentEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/Enrollment/v2/{id:int}/transfer/{newTrackId:int}",
                async (int id, int newTrackId, IMediator mediator) =>
                {
                    var response = await mediator.Send(
                        new TransferEnrollmentOrchestrator(id, newTrackId));

                    if (!response.Success)
                        return Results.Json(EndpointResponse.FromResult(response),statusCode: response.StatusCode);

                    return Results.Ok(EndpointResponse.Ok(response.Message));
                })
            .WithTags("Enrollment V2")
            .WithName("TransferEnrollment");
        }

    }
}
