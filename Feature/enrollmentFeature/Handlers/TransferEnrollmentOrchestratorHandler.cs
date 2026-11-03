using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using LMS___Mini_Version.Feature.Tracks.Query;
using LMS___Mini_Version.Messages;
using MassTransit;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public class TransferEnrollmentOrchestratorHandler:IRequestHandler<TransferEnrollmentOrchestrator,RequestResponse<int>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishEndpoint _publishEndpoint;
        public TransferEnrollmentOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<RequestResponse<int>>Handle(TransferEnrollmentOrchestrator request,CancellationToken cancellationToken)
        {
            var enrollmentResponse = await _mediator.Send(new GetEnrollmentByIdQuery(request.PreviousEnrollmentID), cancellationToken);
            if(!enrollmentResponse.Success|| enrollmentResponse.Data == null)
                return RequestResponse<int>.Fail("Enrollment not found",404);
            
            var enrollment = enrollmentResponse.Data;    

            if(enrollment.Status != EnrollmentStatus.Active)
                return RequestResponse<int>.Fail("Enrollment is not active", 400);

            var newTackResponse = await _mediator.Send(new GetByIdTrackQuery(request.newTrackID), cancellationToken);
            if(!newTackResponse.Success || newTackResponse.Data == null)
                return RequestResponse<int>.Fail("New Track not found",404);

            //new track
            var newTrack = newTackResponse.Data;

            if (!newTrack.IsActive)
                return RequestResponse<int>.Fail("New Track is not active", 400);

            if (newTrack.MaxCapacity <= newTrack.CurrentEnrollmentCount)
                return RequestResponse<int>.Fail("New Track has no available capacity",400);

            if(enrollment.TrackId == request.newTrackID)
                return RequestResponse<int>.Fail("Intern is already enrolled in the new track.", 400);

            var existingEnrollmentResponse = await _mediator.Send(new GetEnrollmentByInternAndTrackQuery( enrollment.InternId, request.newTrackID), cancellationToken);
            if (!existingEnrollmentResponse.Success)
                return RequestResponse<int>.Fail("Failed to verify existing enrollment.", 500);

            if (existingEnrollmentResponse.Data)
                return RequestResponse<int>.Fail("Intern is already enrolled in the new track.", 400);

            //int newEnrollmentId = 0;
            await _unitOfWork.ExecuteAsync(async ct =>
            {
                await _mediator.Send(new UpdateEnrollmentTrackCommand(enrollment.Id, request.newTrackID), ct);

                await _publishEndpoint.Publish(new EnrollmentTransferred(enrollment.InternId, enrollment.Id, enrollment.TrackId, request.newTrackID, DateTime.UtcNow), ct);

            }, cancellationToken);

            return RequestResponse<int>.Ok(enrollment.Id);
        }
    }
}
