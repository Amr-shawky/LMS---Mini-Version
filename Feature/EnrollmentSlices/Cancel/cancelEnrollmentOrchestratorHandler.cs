using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers;

public sealed class cancelEnrollmentOrchestratorHandler(IMediator mediator, IUnitOfWork uow)
    : IRequestHandler<cancelEnrollmentOrchestrator, RequestResponse>
{
    public async Task<RequestResponse> Handle(cancelEnrollmentOrchestrator req, CancellationToken ct)
    {
        var found = await mediator.Send(new GetEnrollmentByIdQuery(req.enrollmentID), ct);
        if (!found.Success || found.Data == null)
            return RequestResponse.Fail("Enrollment not found", 404);
        if (found.Data.Status is EnrollmentStatus.Cancelled or EnrollmentStatus.Completed)
            return RequestResponse.Fail("Enrollment cannot be cancelled in its current status", 400);

        await uow.ExecuteAsync(async () =>
        {
            var status = await mediator.Send(new updateEnrollmentStatusCommand(req.enrollmentID, EnrollmentStatus.Cancelled), ct);
            if (!status.Success)
                throw new InvalidOperationException(status.Message);

            var refund = await mediator.Send(new refundPaymentCommand(req.enrollmentID), ct);
            if (!refund.Success)
                throw new InvalidOperationException(refund.Message);
            if (refund.Data > 0)
            {
                var history = await mediator.Send(new CreateRefundHistoryCommand(refund.Data, req.enrollmentID), ct);
                if (!history.Success)
                    throw new InvalidOperationException(history.Message);
            }
        });
        return RequestResponse.Ok("Enrollment cancelled successfully");
    }
}
