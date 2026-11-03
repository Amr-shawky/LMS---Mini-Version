using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.endpoints
{
    public static class TransferEnrollmentEndpoint
    {
        public static void MapTransferEnrollmentEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/Enrollment/v2/{PreviousEnrollmentID:int}/transfer", async (int PreviousEnrollmentID, int newTrackID, IMediator mediator) =>
            {
                var response = await mediator.Send(new TransferEnrollmentOrchestrator(PreviousEnrollmentID, newTrackID));
                if (!response.Success)
                    return Results.BadRequest(EndpointResponse<int>.Fail(response.Message, response.StatusCode, response.Errors));

                return Results.Ok(EndpointResponse<int>.Ok(response.Data));
            }).WithTags("Enrollment V2")
            .WithSummary("Transfer an enrollment (v2)");
        }
    }
}
