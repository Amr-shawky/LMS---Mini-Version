using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Common;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Features.Enrollments.Queries;
using LMS___Mini_Version.Features.Payments.Commands;
using LMS___Mini_Version.Features.Tracks.Queries;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Orchestrators
{
    public class TransferEnrollmentOrchestratorHandler : IRequestHandler<TransferEnrollmentOrchestratorRequest, CommandResult>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public TransferEnrollmentOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(TransferEnrollmentOrchestratorRequest request, CancellationToken cancellationToken)
        {
            var enrollment = await _mediator.Send(new GetEnrollmentByIdQuery(request.EnrollmentId), cancellationToken);
            if (enrollment == null)
                return CommandResult.Fail("Enrollment not found.");

            if (enrollment.Status == EnrollmentStatus.Cancelled.ToString())
                return CommandResult.Fail("Cannot transfer a cancelled enrollment.");
            

            var newTrack = await _mediator.Send(new GetTrackByIdQuery(request.NewTrackId), cancellationToken);
            if (newTrack == null)
                return CommandResult.Fail("Track not found.");

            if (!newTrack.IsActive)
                return CommandResult.Fail("Cannot transfer to an inactive track.");

            var hasCapacity = await _mediator.Send(new CheckTrackCapacityQuery(request.NewTrackId), cancellationToken);
            if (!hasCapacity)
                return CommandResult.Fail("The new track is at full capacity.");

            var trackUpdated = await _mediator.Send(new StageUpdateEnrollmentTrackCommand(request.EnrollmentId, request.NewTrackId), cancellationToken);
            if (!trackUpdated)
                return CommandResult.Fail("Failed to update enrollment track.");
    
            if (newTrack.Fees > 0)
                await _mediator.Send(new StageUpdatePaymentAmountCommand(request.EnrollmentId,newTrack.Fees), cancellationToken);
            
            await _unitOfWork.CompleteAsync();

            return CommandResult.Succeed(
                $"Enrollment transferred to '{newTrack.Name}' successfully. New fees: {newTrack.Fees:C}.");
        }
    }
}
