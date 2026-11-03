using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using LMS___Mini_Version.Feature.enrollmentFeature.Messaging;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using LMS___Mini_Version.Feature.Tracks.Query;
using MassTransit;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public sealed class TransferEnrollmentOrchestratorHandler : IRequestHandler<TransferEnrollmentOrchestrator, RequestResponse>
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

        public async Task<RequestResponse> Handle(TransferEnrollmentOrchestrator request, CancellationToken cancellationToken)
        {
            var enrollmentResponse = await _mediator.Send(new GetEnrollmentByIdQuery(request.EnrollmentID), cancellationToken);
            if (!enrollmentResponse.Success || enrollmentResponse.Data is null)
                return RequestResponse.Fail("Enrollment was not found.", 404);

            var enrollment = enrollmentResponse.Data;
            if (enrollment.Status == EnrollmentStatus.Cancelled)
                return RequestResponse.Fail("A cancelled enrollment cannot be transferred.", 400);
            if (enrollment.TrackId == request.newTrackID)
                return RequestResponse.Fail("The destination track must be different.", 400);

            var trackResponse = await _mediator.Send(new GetByIdTrackQuery(request.newTrackID), cancellationToken);
            if (!trackResponse.Success || trackResponse.Data is null)
                return RequestResponse.Fail("Destination track was not found.", 404);
            if (!trackResponse.Data.IsActive)
                return RequestResponse.Fail("Destination track is not active.", 400);

            var activeCount = await _mediator.Send(new getActiveEnrollmentCountByTrackQuery(request.newTrackID), cancellationToken);
            if (activeCount >= trackResponse.Data.MaxCapacity)
                return RequestResponse.Fail("Destination track has reached its maximum capacity.", 400);

            await _unitOfWork.ExecuteAsync(async () =>
            {
                var update = await _mediator.Send(new updateEnrollmentTrackCommand(request.EnrollmentID, request.newTrackID), cancellationToken);
                if (!update.Success)
                    throw new InvalidOperationException(update.Message);

                await _publishEndpoint.Publish(new EnrollmentTransferred(request.EnrollmentID, enrollment.TrackId, request.newTrackID), cancellationToken);
            });

            return RequestResponse.Ok("Enrollment transferred successfully");
        }
    }
}
