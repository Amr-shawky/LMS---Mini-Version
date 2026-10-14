using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.Tracks.Query;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Track;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LMS___Mini_Version.Feature.Tracks.endpoints
{
    public static class GetTrackByIdEndpoint
    {
        public static void MapGetTrackByIdEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/Track/v2/{id:int}", async (int id, IMediator mediator) =>
            {
                var response = await mediator.Send(new GetByIdTrackQuery(id));
                if (!response.Success || response.Data == null)
                {
                    return Results.NotFound(EndpointResponse<TrackDetailViewModel>.Fail(response.Message, response.StatusCode, response.Errors));
                }

                return Results.Ok(EndpointResponse<TrackDetailViewModel>.Ok(response.Data.ToDetailViewModel()));
            }).WithTags("Track V2")
            .WithSummary("Get track by ID (v2)");
        }
    }
}
