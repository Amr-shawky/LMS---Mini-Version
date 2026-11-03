using exam_system.Features.Shared;
using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LMS___Mini_Version.Feature.enrollmentFeature.EnrollIntern
{
    public static class EnrollInternEndpoint
    {
        public static void MapEnrollInternEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/Enrollment/v2", async (EnrollInternViewModel vm, IMediator mediator) =>
            {
                var response = await mediator.Send(new EnrollInternOrchestrator(vm.InternId, vm.TrackId));
                if (!response.Success)
                {
                    return Results.BadRequest(EndpointResponse<int>.Fail(response.Message, response.StatusCode, response.Errors));
                }

                return Results.Created($"/api/Enrollment/v2/{response.Data}", EndpointResponse<int>.Created(response.Data, "Enrollment created successfully"));
            }).WithTags("Enrollment V2")
            .WithSummary("Enroll an intern into a track (v2)");
        }
    }
}

