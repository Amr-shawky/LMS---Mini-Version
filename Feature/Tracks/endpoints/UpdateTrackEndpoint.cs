using LMS___Mini_Version.Feature.Tracks.Commands;
using LMS___Mini_Version.Feature.Tracks.ViewModels;
using MediatR;

namespace LMS___Mini_Version.Feature.Tracks.endpoints
{
    public static class UpdateTrackEndpoint
    {
        public static void MapUpdateTrackEndpoint(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("api/v1/tracks")
                .WithTags("Track");

            group.MapPut("/", async (IMediator mediator ,UpdateTrackVM VM) => {

                var result =await mediator.Send(new UpdateTrackCommand(VM.Id,VM.Name,VM.Fees,VM.IsActive,VM.MaxCapacity));

                return Results.Ok("updated");
            });
        }
    }
}
