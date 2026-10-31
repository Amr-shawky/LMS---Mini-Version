using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.endpoints
{
    public static class EnrollInternEndPoint
    {
        public static void MapEnrollInternEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/Enrollment/v2", async (EnrollInternOrchestrator request, IMediator mediator) =>
            {
                var response = await mediator.Send(request);

                if (!response.Success)
                    return Results.Json(EndpointResponse<int>.FromResult(response),statusCode: response.StatusCode);

                return Results.Created($"/api/Enrollment/v2/{response.Data}",EndpointResponse<int>.Created(response.Data, response.Message));
            })
            .WithTags("Enrollment V2")
            .WithName("EnrollIntern");
        }
    }
}
