using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace LMS___Mini_Version.Feature.enrollmentFeature.endpoints
{
    public static class EnrollmentV2Endpoints
    {
        public static void MapEnrollmentV2Endpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/Enrollment/v2").WithTags("Enrollment V2");

            group.MapGet("", async (IMediator mediator, int pageIndex = 1, int pageSize = 10) =>
            {
                var result = await mediator.Send(new GetAllEnrollmentsQuery(pageIndex, pageSize));
                if (!result.Success || result.Data is null)
                    return Results.Json(EndpointResponse<PaginatedResult<EnrollmentViewModel>>.Fail(result.Message, result.StatusCode, result.Errors), statusCode: result.StatusCode);

                var items = result.Data.Items.Select(x => x.ToViewModel()).ToList();
                var data = PaginatedResult<EnrollmentViewModel>.Create(items, result.Data.TotalCount, result.Data.PageIndex, result.Data.PageSize);
                return Results.Ok(EndpointResponse<PaginatedResult<EnrollmentViewModel>>.Ok(data));
            });

            group.MapGet("{id:int}", async (int id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetEnrollmentByIdQuery(id));
                if (!result.Success || result.Data is null)
                    return Results.Json(EndpointResponse<EnrollmentViewModel>.Fail(result.Message, result.StatusCode, result.Errors), statusCode: result.StatusCode);

                return Results.Ok(EndpointResponse<EnrollmentViewModel>.Ok(result.Data.ToViewModel()));
            });

            group.MapPost("", async (EnrollInternViewModel model, IMediator mediator) =>
            {
                var result = await mediator.Send(new EnrollInternOrchestrator(model.InternId, model.TrackId));
                if (!result.Success)
                    return Results.Json(EndpointResponse<int>.Fail(result.Message, result.StatusCode, result.Errors), statusCode: result.StatusCode);

                return Results.Json(EndpointResponse<int>.Created(result.Data, "Enrollment created successfully"), statusCode: 201);
            });

            group.MapPost("{id:int}/cancel", async (int id, IMediator mediator) =>
            {
                var result = await mediator.Send(new cancelEnrollmentOrchestrator(id));
                if (!result.Success)
                    return Results.Json(EndpointResponse.FromResult(result), statusCode: result.StatusCode);

                return Results.Ok(EndpointResponse.Ok("Enrollment cancelled successfully"));
            });

            group.MapPost("{id:int}/transfer/{newTrackId:int}", async (int id, int newTrackId, IMediator mediator) =>
            {
                var result = await mediator.Send(new TransferEnrollmentOrchestrator(id, newTrackId));
                if (!result.Success)
                    return Results.Json(EndpointResponse.FromResult(result), statusCode: result.StatusCode);

                return Results.Ok(EndpointResponse.Ok("Enrollment transferred successfully"));
            });
        }
    }
}
