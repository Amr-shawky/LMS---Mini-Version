using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LMS___Mini_Version.Feature.enrollmentFeature.endpoints
{
    public static class EnrollEndpoint
    {
        public static void MapEnrollEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/Enrollment/v2", async (EnrollInternViewModel vm, IMediator mediator) =>
            {
                var result = await mediator.Send(new EnrollInternOrchestrator(vm.InternId, vm.TrackId));
                if (!result.Success)
                    return Results.Json(
                        EndpointResponse<int>.Fail(result.Message, result.StatusCode, result.Errors),
                        statusCode: result.StatusCode);

                // R6: return the enrollment ID. R7: wrap in EndpointResponse<int> with Created status.
                return Results.Json(
                    EndpointResponse<int>.Created(result.Data, "Enrollment created successfully."),
                    statusCode: 201);
            })
            .WithTags("Enrollment V2")
            .WithSummary("Enroll an intern in a track (v2)");
        }
    }
}