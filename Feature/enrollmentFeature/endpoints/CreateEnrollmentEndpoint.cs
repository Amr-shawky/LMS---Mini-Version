using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.endpoints
{
    public static class CreateEnrollmentEndpoint
    {
        public static void MapCreateEnrollmentEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/Enrollment/v2", async (IMediator mediator, EnrollInternOrchestrator request) =>
            {
                var response = await mediator.Send(request);
                if (!response.Success)
                {
                    return Results.BadRequest(EndpointResponse<int>.Fail(response.Message, response.StatusCode, response.Errors));
                }
                return Results.Created($"api/Enrollment/v2/{response.Data}", EndpointResponse<int>.Created(response.Data));
            }).WithTags("Enrollment V2")
            .WithSummary("Create a new enrollment (v2)");
        }
    }
}

