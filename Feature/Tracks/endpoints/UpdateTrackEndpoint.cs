using LMS___Mini_Version.Feature.Tracks.Commands;
using LMS___Mini_Version.ViewModels.Track;
using MediatR;
using LMS___Mini_Version.Feature.Tracks.ViewModels;

namespace LMS___Mini_Version.Feature.Tracks.endpoints
{
    public static class UpdateTrackEndpoint
    {
        public static void MapUpdateTrackEndpoint(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/Track/v0")
                .WithTags("Track");


            group.MapPut("/", async (IMediator mediator, UpdateTrackVM vm) =>
            {
                var result = await mediator.Send(new UpdateTrackCommand(vm.Id, vm.Name, vm.Fees, vm.IsActive, vm.MaxCapacity));

                return Results.Ok();
            });
            
        }
    }
}
