using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.Shared;
using LMS___Mini_Version.Feature.Tracks.Commands;
using LMS___Mini_Version.Feature.Tracks.Query;
using LMS___Mini_Version.ViewModels.Track;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LMS___Mini_Version.Feature.Tracks.endpoints.test
{
    public class TrackTestEndpoints : EndpointDefinition
    {
        public override void RegisterEndpoints(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/test/tracks").WithTags("Tracks (Test Minimal API)");

            group.MapGet("/", GetAll);
            group.MapPost("/", Create);
        }

        // ─────────────────────────────────────────────────────────────
        // 1) GET ALL TRACKS (Query - No Transaction Needed)
        // ─────────────────────────────────────────────────────────────
        private static async Task<IResult> GetAll(
            IMediator mediator,
            int pageIndex = 1,
            int pageSize = 10)
        {
            var queryResult = await mediator.Send(new GetAllTrackQuery(pageIndex, pageSize));
            return Response(queryResult);
        }

        // ─────────────────────────────────────────────────────────────
        // 2) CREATE TRACK (Command - Runs within IUnitOfWork Transaction)
        // ─────────────────────────────────────────────────────────────
        private static async Task<IResult> Create(
            CreateTrackViewModel vm,
            IMediator mediator,
            HttpContext httpContext)
        {
            return await ExecuteWithTransactionAsync<string>(async () =>
            {
                await mediator.Send(new CreateTrackCommand(
                    vm.Name,
                    vm.Fees,
                    vm.IsActive,
                    vm.MaxCapacity
                ));

                return "Track created successfully in transaction!";
            }, httpContext);
        }
    }

    public static class TrackTestEndpointExtensions
    {
        public static void MapTrackTestEndpoints(this IEndpointRouteBuilder app)
        {
            new TrackTestEndpoints().RegisterEndpoints(app);
        }
    }
}
