using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.Tracks.Commands;
using LMS___Mini_Version.Feature.Tracks.Query;
using LMS___Mini_Version.ViewModels.Track;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Feature.Tracks.endpoints
{
    public static class GetAlltracksEndpoint
    {
        public static void MapGetAlltracksEndpoint(this WebApplication app)
        
        {
            var group = app.MapGroup("api/v2/tracks")
                           .WithTags("Track");
                           
            group.MapGet("/", async (IMediator mediator, int PageIndex = 1, int PageSize = 10) => {


                var response = await mediator.Send(new GetAllTrackQuery(PageIndex, PageSize));
                var trackvm = response.Data.Items.Select(t => new TrackSummaryViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Fees = t.Fees,
                    IsActive = t.IsActive
                }).ToList();

                var paginatedresult = PaginatedResult<TrackSummaryViewModel>.Create(
                    trackvm,
                    response.Data.TotalCount,
                    response.Data.PageIndex,
                    response.Data.PageSize

                    ); 

                var result = EndpointResponse<PaginatedResult<TrackSummaryViewModel>>.Ok(paginatedresult);

                return result;

            })
                .WithName("GetAllTracks")
                .WithSummary("Get paginated list of tracks")
                .WithDescription("Retrieves active tracks with summary details including fees and status.")
                .Produces<EndpointResponse<PaginatedResult<TrackSummaryViewModel>>>(StatusCodes.Status200OK);


            group.MapPost("/", async (IMediator mediator , CreateTrackViewModel vm ) => {

                var request = await mediator.Send(new CreateTrackCommand(vm.Name, vm.Fees, vm.IsActive, vm.MaxCapacity));


                return Results.Ok();
            });
        }
    }
}
