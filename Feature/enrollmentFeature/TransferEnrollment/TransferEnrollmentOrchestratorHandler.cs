using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.EnrollIntern;
using LMS___Mini_Version.Feature.enrollmentFeature.GetEnrollmentById;
using LMS___Mini_Version.Feature.Tracks.Query;
using MassTransit;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.TransferEnrollment
{
    public class TransferEnrollmentOrchestratorHandler : IRequestHandler<TransferEnrollmentOrchestrator, RequestResponse>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishEndpoint _publishEndpoint;

        public TransferEnrollmentOrchestratorHandler(
            IMediator mediator,
            IUnitOfWork unitOfWork,
            IPublishEndpoint publishEndpoint)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<RequestResponse> Handle(TransferEnrollmentOrchestrator request, CancellationToken cancellationToken)
        {
            // Step 1: Validate existing enrollment
            var enrollmentResponse = await _mediator.Send(new GetEnrollmentByIdQuery(request.EnrollmentID), cancellationToken);
            if (!enrollmentResponse.Success || enrollmentResponse.Data == null)
            {
                return RequestResponse.Fail($"Enrollment with ID {request.EnrollmentID} was not found.", 404);
            }

            var enrollment = enrollmentResponse.Data;

            if (enrollment.Status == EnrollmentStatus.Cancelled)
            {
                return RequestResponse.Fail("Cannot transfer a cancelled enrollment.", 400);
            }

            if (enrollment.TrackId == request.newTrackID)
            {
                return RequestResponse.Fail("Enrollment is already in the requested destination track.", 400);
            }

            // Step 2: Validate destination track exists and is active
            var destinationTrackResponse = await _mediator.Send(new GetByIdTrackQuery(request.newTrackID), cancellationToken);
            if (!destinationTrackResponse.Success || destinationTrackResponse.Data == null)
            {
                return RequestResponse.Fail($"Destination Track with ID {request.newTrackID} was not found.", 404);
            }

            var destinationTrack = destinationTrackResponse.Data;
            if (!destinationTrack.IsActive)
            {
                return RequestResponse.Fail($"Destination Track '{destinationTrack.Name}' is not currently active.", 400);
            }

            // Step 3: Validate destination track capacity
            var activeCount = await _mediator.Send(new getActiveEnrollmentCountByTrackQuery(request.newTrackID), cancellationToken);
            if (activeCount >= destinationTrack.MaxCapacity)
            {
                return RequestResponse.Fail($"Destination Track '{destinationTrack.Name}' has reached maximum capacity.", 400);
            }

            // Step 4: Atomic update & outbox publish in the same SQL transaction
            await _unitOfWork.ExecuteAsync(async () =>
            {
                // 4a. Update Enrollment Track ID via SaveInclude
                await _mediator.Send(new updateEnrollmentTrackCommand(request.EnrollmentID, request.newTrackID), cancellationToken);

                // 4b. Publish MassTransit Outbox Event (persisted transactionally with DB update)
                await _publishEndpoint.Publish(new EnrollmentTransferredEvent(
                    request.EnrollmentID,
                    enrollment.InternId,
                    enrollment.TrackId,
                    request.newTrackID,
                    DateTime.UtcNow
                ), cancellationToken);
            });

            return RequestResponse.Ok("Enrollment transferred successfully.");
        }
    }
}

