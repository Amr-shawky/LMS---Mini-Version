using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Queries;
using LMS___Mini_Version.Features.Tracks.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Commands
{
    public record TransferEnrollmentOrchestrator(int EnrollmentId, int NewTrackId) : IRequest;


    public class TransferEnrollmentCommandHandler : IRequestHandler<TransferEnrollmentOrchestrator, Unit>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;
        private readonly IGeneralRepository<Track> _trackRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public TransferEnrollmentCommandHandler(
            IGeneralRepository<Enrollment> enrollmentRepository,
            IGeneralRepository<Track> trackRepository,
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _enrollmentRepository = enrollmentRepository;
            _trackRepository = trackRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        // 1- check enrollment is found 
        // 2- check this enrollment is canceld to move to another track 
        // 3- check the new track is not the same track that the intern enrollment in it
        // 4- check the New track found , active and the maxCapacity is availble to intern enroll in this track  
        // 5- update the enrollment track with new track intern selected and update payment
        public async Task<Unit> Handle(TransferEnrollmentOrchestrator request, CancellationToken cancellationToken)
        {
            var enrollment = await _mediator.Send(new GetEnrollmentByIdQuery(request.EnrollmentId));

            if (enrollment == null)
                throw new KeyNotFoundException($"Enrollment was not found.");

            if (enrollment.Status == EnrollmentStatus.Cancelled)
                throw new InvalidOperationException("Cannot transfer a cancelled enrollment.");

            if (enrollment.TrackId == request.NewTrackId)
                throw new InvalidOperationException("The intern is already enrolled in this track.");

            var newTrack = await _mediator.Send(new GetTrackByIdQuery(request.NewTrackId));
            if (newTrack == null)
                throw new KeyNotFoundException($"Target Track not found.");

            if (!newTrack.IsActive)
                throw new InvalidOperationException($"Target Track is not active.");

            var activeCount = await _mediator.Send(new GetActiveEnrollmentCountByTrackQuery(request.NewTrackId),cancellationToken);

            if (activeCount >= newTrack.MaxCapacity)
                throw new InvalidOperationException($"Target Track has reached its maximum capacity.");


               await _mediator.Send(new UpdateEnrollmentTrackCommand(request.EnrollmentId, request.NewTrackId), cancellationToken);

            if (newTrack.Fees > 0)
                await _mediator.Send(new UpdatePaymentAmountCommand(request.EnrollmentId, newTrack.Fees), cancellationToken);
            
            //atomic commit in db for all actions
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}