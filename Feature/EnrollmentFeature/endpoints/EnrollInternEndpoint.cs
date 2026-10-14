using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LMS___Mini_Version.Feature.enrollmentFeature.endpoints
{
    public static class EnrollInternEndpoint
    {
        public static void MapEnrollInternEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/Enrollment/v2", async (EnrollInternViewModel vm, IMediator mediator) =>
            {
                var result = await mediator.Send(new EnrollInternOrchestrator(vm.InternId, vm.TrackId));
                if (!result.Success || result.Data == null)
                {
                    return Results.BadRequest(EndpointResponse<EnrollmentViewModel>.Fail(result.Message, result.StatusCode, result.Errors));
                }

                return Results.Created($"/api/Enrollment/v2/{result.Data.Id}", EndpointResponse<EnrollmentViewModel>.Created(result.Data.ToViewModel(), "Enrollment created successfully"));
            }).WithTags("Enrollment V2")
            .WithSummary("Enroll an intern (v2)");
        }
    }
}
