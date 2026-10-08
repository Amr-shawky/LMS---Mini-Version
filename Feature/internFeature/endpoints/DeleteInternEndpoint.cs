using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.internFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LMS___Mini_Version.Feature.internFeature.endpoints
{
    public static class DeleteInternEndpoint
    {
        public static void MapDeleteInternEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapDelete("api/Intern/v2/{id:int}", async (int id, IMediator mediator) =>
            {
                var response = await mediator.Send(new DeleteInternCommand(id));
                if (!response.Success)
                {
                    return Results.NotFound(EndpointResponse.Fail(response.Message, response.StatusCode, response.Errors));
                }

                return Results.Ok(EndpointResponse.Ok("Intern deleted successfully"));
            }).WithTags("Intern V2")
            .WithSummary("Delete intern (v2)");
        }
    }
}
