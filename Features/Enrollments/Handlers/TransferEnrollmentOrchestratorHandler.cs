using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Features.Enrollments.Orchestrators;
using LMS___Mini_Version.Features.Enrollments.Queries;
using LMS___Mini_Version.Features.Tracks.Queries;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Handlers
{
    public class TransferEnrollmentOrchestratorHandler : IRequestHandler<TransferEnrollmentOrchestrator>
    {
        private readonly IMediator _mediator;

        public TransferEnrollmentOrchestratorHandler(IMediator mediator)

        => _mediator = mediator;
        public async Task<Unit> Handle(TransferEnrollmentOrchestrator request, CancellationToken cancellationToken)
        {
            var enrollment = await _mediator.Send(new GetEnrollmentsByIdQuery(request.EnrollmentID));
            if (enrollment.Status == EnrollmentStatus.Cancelled)
                throw new InvalidOperationException("Cannot Transfer Enrollment Canceled");
            if (enrollment.TrackId == request.newTrackID)
                throw new InvalidOperationException("the Intern Already Enrolled This Track ");
            var track = await _mediator.Send(new GetTrackByIdQuery(request.newTrackID));
            if (!track.IsActive) throw new InvalidOperationException($"Track{request.newTrackID} Not Found");
            var activeEnrolledCount = await _mediator.Send(new GetActiveEnrollmentCountByTrackQuery(request.newTrackID));
            if(activeEnrolledCount >= track.MaxCapacity)
                throw new InvalidOperationException("Target track has reached maximum capacity");

            await _mediator.Send(new UpdateEnrollmentTrackCommand(request.EnrollmentID, request.newTrackID));
            if (track.Fees > 0)
                await _mediator.Send(new RefundPaymentCommand(request.EnrollmentID));
            return Unit.Value;
        }
    }
}
