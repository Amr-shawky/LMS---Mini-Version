using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using LMS___Mini_Version.Feature.internFeature.Queries;
using LMS___Mini_Version.Feature.paymentFeature.Commands;
using LMS___Mini_Version.Feature.Tracks.Query;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers;

public sealed class EnrollInternOrchestratorHandler(IMediator mediator, IUnitOfWork uow)
    : IRequestHandler<EnrollInternOrchestrator, RequestResponse<int>>
{
    public async Task<RequestResponse<int>> Handle(EnrollInternOrchestrator req, CancellationToken ct)
    {
        var intern = await mediator.Send(new GetInternByIdQuery(req.InternId), ct);
        if (!intern.Success || intern.Data == null)
            return RequestResponse<int>.Fail("Intern not found", 404);

        var track = await mediator.Send(new GetByIdTrackQuery(req.TrackId), ct);
        if (!track.Success || track.Data == null)
            return RequestResponse<int>.Fail("Track not found", 404);
        if (!track.Data.IsActive)
            return RequestResponse<int>.Fail("Track is inactive");

        var duplicate = await mediator.Send(new HasActiveEnrollmentQuery(req.InternId, req.TrackId), ct);
        if (duplicate.Data)
            return RequestResponse<int>.Fail("Intern is already enrolled in this track", 409);

        var countResult = await mediator.Send(new getActiveEnrollmentCountByTrackQuery(req.TrackId), ct);
        if (!countResult.Success) return RequestResponse<int>.Fail(countResult.Message, countResult.StatusCode);
        if (countResult.Data >= track.Data.MaxCapacity)
            return RequestResponse<int>.Fail("Track is at capacity", 409);

        int enrollmentId = 0;
        await uow.ExecuteAsync(async () =>
        {
            var created = await mediator.Send(new CreateEnrollmentCommand(req.InternId, req.TrackId), ct);
            if (!created.Success || created.Data <= 0)
                throw new InvalidOperationException(created.Message);
            enrollmentId = created.Data;
            if (track.Data.Fees > 0)
            {
                var payment = await mediator.Send(new CreatePaymentCommand(
                    enrollmentId, track.Data.Fees, PaymentMethod.Cash, PaymentStatus.Pending), ct);
                if (!payment.Success)
                    throw new InvalidOperationException(payment.Message);
            }
        });
        return RequestResponse<int>.Created(enrollmentId);
    }
}
