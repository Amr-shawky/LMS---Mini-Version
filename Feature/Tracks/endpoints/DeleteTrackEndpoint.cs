using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.Tracks.Commands;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LMS___Mini_Version.Feature.Tracks.endpoints
{
    public static class DeleteTrackEndpoint
    {
        public static void MapDeleteTrackEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapDelete("api/Track/v2/{id:int}", async (int id, IMediator mediator) =>
            {
                var response = await mediator.Send(new DeleteTrackCommand(id));
                if (!response.Success)
                {
                    return Results.NotFound(EndpointResponse.Fail(response.Message, response.StatusCode, response.Errors));
                }

                return Results.Ok(EndpointResponse.Ok("Track deleted successfully"));
            }).WithTags("Track V2")
            .WithSummary("Delete track (v2)");
        }
    }
}
