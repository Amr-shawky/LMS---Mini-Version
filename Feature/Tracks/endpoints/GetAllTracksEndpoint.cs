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
    public static class GetAllTracksEndpoint
    {
        public static void MapGetAllTracksEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/Track/v2", async (IMediator mediator, int pageIndex = 1, int pageSize = 10) =>
            {
                var response = await mediator.Send(new GetAllTrackQuery(pageIndex, pageSize));
                if (!response.Success || response.Data == null)
                {
                    return Results.BadRequest(EndpointResponse<PaginatedResult<TrackSummaryViewModel>>.Fail(response.Message, response.StatusCode, response.Errors));
                }

                var trackvm = response.Data.Items.Select(t => t.ToSummaryViewModel()).ToList();
                var paginatedResult = PaginatedResult<TrackSummaryViewModel>.Create(
                    trackvm,
                    response.Data.TotalCount,
                    response.Data.PageIndex,
                    response.Data.PageSize
                );

                return Results.Ok(EndpointResponse<PaginatedResult<TrackSummaryViewModel>>.Ok(paginatedResult));
            }).WithTags("Track V2")
            .WithSummary("Get all tracks paginated (v2)");
        }
    }
}
