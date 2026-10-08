using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.Tracks.Commands;
using LMS___Mini_Version.Feature.Tracks.Query;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Track;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LMS___Mini_Version.Feature.Tracks.endpoints
{
    public static class CreateTrackEndpoint
    {
        public static void MapCreateTrackEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/Track/v2", async (CreateTrackViewModel vm, IMediator mediator) =>
            {
                var createResponse = await mediator.Send(new CreateTrackCommand(vm.Name, vm.Fees, vm.IsActive, vm.MaxCapacity));
                if (!createResponse.Success)
                {
                    return Results.BadRequest(EndpointResponse<TrackSummaryViewModel>.Fail(createResponse.Message, createResponse.StatusCode, createResponse.Errors));
                }

                var trackResponse = await mediator.Send(new GetByIdTrackQuery(createResponse.Data));
                var summaryVm = trackResponse.Data != null
                    ? trackResponse.Data.ToSummaryViewModel()
                    : new TrackSummaryViewModel { Id = createResponse.Data, Name = vm.Name, Fees = vm.Fees, IsActive = vm.IsActive };

                return Results.Created($"/api/Track/v2/{createResponse.Data}", EndpointResponse<TrackSummaryViewModel>.Created(summaryVm, "Track created successfully"));
            }).WithTags("Track V2")
            .WithSummary("Create track (v2)");
        }
    }
}
