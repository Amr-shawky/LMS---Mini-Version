using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;

namespace LMS___Mini_Version.Feature.EnrollmentFeature.endpoints
{
    public static class EnrollInternEndpoint
    {
        public static void MapEnrollInternEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/Enrollment/v2", async (EnrollInternViewModel model, IMediator mediator) =>
            {
                var response = await mediator.Send(
                    new EnrollInternOrchestrator(model.InternId, model.TrackId));

                if (!response.Success || response.Data == null)
                {
                    return Results.Json(
                        EndpointResponse<int>.Fail(
                            response.Message, response.StatusCode, response.Errors),
                        statusCode: response.StatusCode);
                }

                return Results.Json(
                    EndpointResponse<int>.Created(response.Data.Id),
                    statusCode: 201);
            })
            .WithTags("Enrollment V2")
            .WithSummary("Enroll an intern in a track (v2)");
        }
    }
}
