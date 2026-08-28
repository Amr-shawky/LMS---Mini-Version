using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Features.Enrollments.Queries;
using LMS___Mini_Version.Features.Tracks.Queries;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Handlers;

public class TransferEnrollmentOrchestratorHandler 
   (IMediator _mediator,
    IUnitOfWork _unitOfWork
    ) : IRequestHandler<TransferEnrollmentOrchestratorCommand, bool>
{


    public async Task<bool> Handle(TransferEnrollmentOrchestratorCommand request, CancellationToken cancellationToken)
    {
        // step 1: get enrollment
        var enrollment = await _mediator.Send(new GetEnrollmentByIdQuery(request.EnrollmentId), cancellationToken);
        if (enrollment == null)
            return false;

        if (enrollment.Status == EnrollmentStatus.Cancelled)
            return false;

        if (enrollment.TrackId == request.NewTrackId)
            return false;

        // step 2: get new track
        var newTrack = await _mediator.Send(new GetTrackByIdQuery(request.NewTrackId), cancellationToken);
        if (newTrack == null || !newTrack.IsActive)
            return false;

        // step 3: check capacity
        var currentCount = await _mediator.Send(new GetActiveEnrollmentsCountQuery(request.NewTrackId), cancellationToken);
        if (currentCount >= newTrack.MaxCapacity)
            return false;

        // step 4: update track (no commit yet)
        await _mediator.Send(new UpdateEnrollmentTrackCommand(request.EnrollmentId, request.NewTrackId), cancellationToken);

        // step 5: adjust payment amount (no commit yet)
        await _mediator.Send(new AdjustPaymentAmountCommand(request.EnrollmentId, newTrack.Fees), cancellationToken);

        // step 6: commit once — atomic
        await _unitOfWork.CompleteAsync();

        return true;
    }
}