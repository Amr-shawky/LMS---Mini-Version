using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.Tracks.Commands;
using LMS___Mini_Version.ViewModels.Track;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LMS___Mini_Version.Feature.Tracks.endpoints
{
    public static class UpdateTrackV2Endpoint
    {
        public static void MapUpdateTrackV2Endpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("api/Track/v2/{id:int}", async (int id, UpdateTrackViewModel vm, IMediator mediator) =>
            {
                var response = await mediator.Send(new UpdateTrackCommand(id, vm.Name, vm.Fees, vm.IsActive, vm.MaxCapacity));
                if (!response.Success)
                {
                    return Results.NotFound(EndpointResponse.Fail(response.Message, response.StatusCode, response.Errors));
                }

                return Results.Ok(EndpointResponse.Ok("Track updated successfully"));
            }).WithTags("Track V2")
            .WithSummary("Update track (v2)");
        }
    }
}
