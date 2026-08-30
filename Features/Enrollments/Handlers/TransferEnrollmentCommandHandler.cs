using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Features.Enrollments.Queries;
using LMS___Mini_Version.Features.Payments.Commands;
using LMS___Mini_Version.Features.Tracks.Queries;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Handlers
{
    public class TransferEnrollmentCommandHandler : IRequestHandler<TransferEnrollmentCommand>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public TransferEnrollmentCommandHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(TransferEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _mediator.Send(new GetEnrollmentByIdQuery(request.EnrollmentId), cancellationToken);
            if (enrollment == null)
            {
                throw new KeyNotFoundException($"Enrollment with ID {request.EnrollmentId} was not found.");
            }

            if (enrollment.Status == EnrollmentStatus.Cancelled)
            {
                throw new InvalidOperationException("Cannot transfer a cancelled enrollment.");
            }

            if (enrollment.TrackId == request.NewTrackId)
            {
                throw new InvalidOperationException("The intern is already enrolled in this track.");
            }

            var newTrack = await _mediator.Send(new GetTrackByIdQuery(request.NewTrackId), cancellationToken);
            if (newTrack == null)
            {
                throw new KeyNotFoundException($"Target Track with ID {request.NewTrackId} not found.");
            }

            if (!newTrack.IsActive)
            {
                throw new InvalidOperationException($"Target Track '{newTrack.Name}' is not currently active.");
            }

            var activeCount = await _mediator.Send(new GetTrackActiveEnrollmentCountQuery(request.NewTrackId), cancellationToken);
            if (activeCount >= newTrack.MaxCapacity)
            {
                throw new InvalidOperationException($"Target Track '{newTrack.Name}' has reached maximum capacity.");
            }

            await _mediator.Send(new UpdateEnrollmentTrackCommand(request.EnrollmentId, request.NewTrackId), cancellationToken);

            if (newTrack.Fees > 0)
            {
                await _mediator.Send(new UpdatePaymentAmountCommand(request.EnrollmentId, newTrack.Fees), cancellationToken);
            }

            await _unitOfWork.CompleteAsync();
        }
    }
}
