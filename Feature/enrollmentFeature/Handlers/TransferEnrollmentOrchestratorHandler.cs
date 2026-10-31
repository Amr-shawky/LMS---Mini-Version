using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using LMS___Mini_Version.Feature.enrollmentFeature.Messaging.Events;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using LMS___Mini_Version.Feature.Tracks.Query;
using MassTransit;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    /// <summary>
    /// Orchestrator for the Transfer workflow (R12-R14, R17, D2).
    ///
    /// Steps:
    ///   1. Validate the enrollment exists and is Active (not Cancelled).
    ///   2. Validate the destination track exists, is Active, and is not the same track.
    ///   3. Check destination track capacity.
    ///   4. Inside ONE UOF transaction (R14):
    ///      a. Update Enrollment.TrackId via updateEnrollmentTrackCommand (uses SaveInclude).
    ///      b. Publish EnrollmentTransferred to the outbox (stored in the same transaction).
    ///      The HTTP response is returned immediately after commit -- it does NOT wait
    ///      for the consumers to process the message (R14).
    ///
    /// The refund is NOT done here. The RefundConsumer handles it asynchronously after
    /// MassTransit delivers the outbox message to RabbitMQ (R13, R19).
    ///
    /// Why publish inside UOF.ExecuteAsync (D2)?
    ///   The outbox table row is written by MassTransit's EF Core outbox in the same
    ///   DbContext transaction. If the DB transaction rolls back, the outbox row is
    ///   also rolled back -- so a failed transfer never emits a spurious event (R22).
    /// </summary>
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
            // Step 1: Validate source enrollment
            var enrollmentResponse = await _mediator.Send(
                new GetEnrollmentByIdQuery(request.EnrollmentID), cancellationToken);

            if (!enrollmentResponse.Success || enrollmentResponse.Data == null)
                return RequestResponse.Fail($"Enrollment {request.EnrollmentID} not found.", 404);

            var enrollment = enrollmentResponse.Data;

            if (enrollment.Status == EnrollmentStatus.Cancelled)
                return RequestResponse.Fail("Cannot transfer a cancelled enrollment.", 400);

            // Step 2: Validate destination track
            if (enrollment.TrackId == request.newTrackID)
                return RequestResponse.Fail("The destination track is the same as the current track.", 400);

            var trackResponse = await _mediator.Send(
                new GetByIdTrackQuery(request.newTrackID), cancellationToken);

            if (!trackResponse.Success || trackResponse.Data == null)
                return RequestResponse.Fail($"Destination track {request.newTrackID} not found.", 404);

            var newTrack = trackResponse.Data;

            if (!newTrack.IsActive)
                return RequestResponse.Fail($"Destination track '{newTrack.Name}' is not active.", 400);

            // Step 3: Check destination track capacity
            var activeCount = await _mediator.Send(
                new getActiveEnrollmentCountByTrackQuery(request.newTrackID), cancellationToken);

            if (activeCount >= newTrack.MaxCapacity)
                return RequestResponse.Fail($"Destination track '{newTrack.Name}' is at full capacity.", 400);

            int oldTrackId = enrollment.TrackId;

            // Step 4: Atomic DB update + outbox publish in one transaction (R14, R22, D2)
            await _unitOfWork.ExecuteAsync(async () =>
            {
                // 4a: Update only the TrackId column via SaveInclude (R11)
                await _mediator.Send(
                    new updateEnrollmentTrackCommand(request.EnrollmentID, request.newTrackID),
                    cancellationToken);

                // 4b: Publish the event. MassTransit's EF Core outbox intercepts this call
                // and writes the serialized message to the outbox table in the same transaction.
                // After commit, the MassTransit delivery service reads the row and forwards
                // the message to RabbitMQ independently (R17, R22).
                await _publishEndpoint.Publish(new EnrollmentTransferred(
                    EnrollmentId: request.EnrollmentID,
                    InternId: enrollment.InternId,
                    OldTrackId: oldTrackId,
                    NewTrackId: request.newTrackID,
                    OccurredAtUtc: DateTime.UtcNow), cancellationToken);
            });

            return RequestResponse.Ok("Enrollment transferred successfully. Refund (if applicable) will be processed asynchronously.");
        }
    }
}