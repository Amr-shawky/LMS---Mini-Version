using LMS___Mini_Version.Features.Track.GetTrackById.Query;
using MediatR;

namespace LMS___Mini_Version.Features.Track.GetTrackById.EndPoint
{
    public static class GetTrackByIdEndPoint
    {
        public static void GetTrackById(this WebApplication app)
        {
            app.MapGet("api/Track/{id}/v2", async (int trackId,IMediator _mediator) =>
            {
              var track =  await _mediator.Send(new GetTrackByIdQuery(trackId));
                if (track == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(track);
            }).WithTags("Tracks");
        }
    }

}
