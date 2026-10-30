using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;

namespace LMS___Mini_Version.Feature.EnrollmentSlices;

public static class EnrollmentEndpoints
{
    public static IEndpointRouteBuilder MapEnrollmentAssignmentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/Enrollment/v2").WithTags("Enrollment V2");

        group.MapGet("", async (IMediator mediator, int pageIndex = 1, int pageSize = 10) =>
        {
            if (pageIndex < 1 || pageSize is < 1 or > 100)
                return Results.BadRequest(EndpointResponse.Fail("Invalid pagination values"));

            var res = await mediator.Send(new GetAllEnrollmentsQuery(pageIndex, pageSize));
            if (!res.Success || res.Data == null)
                return Results.Json(EndpointResponse.Fail(res.Message, res.StatusCode), statusCode: res.StatusCode);
            var items = res.Data.Items.Select(e => e.ToViewModel()).ToList();
            var page = PaginatedResult<EnrollmentViewModel>.Create(items,
                res.Data.TotalCount, res.Data.PageIndex, res.Data.PageSize);
            return Results.Ok(EndpointResponse<PaginatedResult<EnrollmentViewModel>>.Ok(page));
        }).WithSummary("List enrollments (paged)");

        group.MapGet("/{id:int}", async (int id, IMediator mediator) =>
        {
            var res = await mediator.Send(new GetEnrollmentByIdQuery(id));
            if (!res.Success || res.Data == null)
                return Results.Json(EndpointResponse.Fail(res.Message, res.StatusCode), statusCode: res.StatusCode);
            return Results.Ok(EndpointResponse<EnrollmentViewModel>.Ok(res.Data.ToViewModel()));
        }).WithSummary("Get enrollment by ID");

        group.MapPost("", async (EnrollInternViewModel vm, IMediator mediator) =>
        {
            var res = await mediator.Send(new EnrollInternOrchestrator(vm.InternId, vm.TrackId));
            if (!res.Success)
                return Results.Json(EndpointResponse.Fail(res.Message, res.StatusCode), statusCode: res.StatusCode);
            return Results.Json(EndpointResponse<int>.Created(res.Data, "Enrollment created"), statusCode: 201);
        }).WithSummary("Enroll intern");

        group.MapPost("/{id:int}/cancel", async (int id, IMediator mediator) =>
        {
            var res = await mediator.Send(new cancelEnrollmentOrchestrator(id));
            return res.Success
                ? Results.Ok(EndpointResponse.Ok(res.Message))
                : Results.Json(EndpointResponse.Fail(res.Message, res.StatusCode), statusCode: res.StatusCode);
        }).WithSummary("Cancel enrollment");

        group.MapPost("/{id:int}/transfer/{newTrackId:int}", async (int id, int newTrackId, IMediator mediator) =>
        {
            var res = await mediator.Send(new TransferEnrollmentOrchestrator(id, newTrackId));
            return res.Success
                ? Results.Ok(EndpointResponse.Ok(res.Message))
                : Results.Json(EndpointResponse.Fail(res.Message, res.StatusCode), statusCode: res.StatusCode);
        }).WithSummary("Transfer enrollment via transactional outbox");

        return app;
    }
}
