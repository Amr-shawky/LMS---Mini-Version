using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.endpoints
{
    public static class cancelEnrollmentEndpoint
    {
         public static void MapCancelEnrollmentEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapDelete("api/Enrollment/v2/{id:int}/cancel",async(int id,IMediator mediator)=>
            {
                var response = await mediator.Send(new cancelEnrollmentOrchestrator(id));
                if(!response.Success)
                    return Results.BadRequest(EndpointResponse.Fail(response.Message, response.StatusCode, response.Errors));

                return Results.Ok(EndpointResponse.Ok());
            }).WithTags("Enrollment V2")
            .WithSummary("Cancel an enrollment (v2)");
        }
    }
}
