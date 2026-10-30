using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using LMS___Mini_Version.Feature.Tracks.Query;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers;

public sealed class TransferEnrollmentOrchestratorHandler(IMediator mediator, IUnitOfWork uow)
    : IRequestHandler<TransferEnrollmentOrchestrator, RequestResponse>
{
    public async Task<RequestResponse> Handle(TransferEnrollmentOrchestrator req, CancellationToken ct)
    {
        var enrollment = await mediator.Send(new GetEnrollmentByIdQuery(req.EnrollmentID), ct);
        if (!enrollment.Success || enrollment.Data == null)
            return RequestResponse.Fail("Enrollment not found", 404);
        if (enrollment.Data.Status != EnrollmentStatus.Active)
            return RequestResponse.Fail("Only active enrollments can be transferred", 400);
        if (enrollment.Data.TrackId == req.newTrackID)
            return RequestResponse.Fail("Source and destination track are the same", 400);

        var target = await mediator.Send(new GetByIdTrackQuery(req.newTrackID), ct);
        if (!target.Success || target.Data == null)
            return RequestResponse.Fail("Destination track not found", 404);
        if (!target.Data.IsActive)
            return RequestResponse.Fail("Destination track is inactive", 400);

        var countResult = await mediator.Send(new getActiveEnrollmentCountByTrackQuery(req.newTrackID), ct);
        if (!countResult.Success) return RequestResponse.Fail(countResult.Message, countResult.StatusCode);
        if (countResult.Data >= target.Data.MaxCapacity)
            return RequestResponse.Fail("Destination track is full", 409);

        await uow.ExecuteAsync(async () =>
        {
            var changed = await mediator.Send(
                new updateEnrollmentTrackCommand(req.EnrollmentID, req.newTrackID), ct);
            if (!changed.Success)
                throw new InvalidOperationException(changed.Message);
        });
        return RequestResponse.Ok("Transfer completed; refund processing is asynchronous");
    }
}
