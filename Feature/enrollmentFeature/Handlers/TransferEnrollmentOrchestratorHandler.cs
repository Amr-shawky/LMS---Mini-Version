using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using LMS___Mini_Version.Feature.Tracks.Query;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public class TransferEnrollmentOrchestratorHandler : IRequestHandler<TransferEnrollmentOrchestrator, RequestResponse>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _uow;

        public TransferEnrollmentOrchestratorHandler(IMediator mediator, IUnitOfWork uow)
        {
            _mediator = mediator;
            _uow = uow;
        }

        public async Task<RequestResponse> Handle(TransferEnrollmentOrchestrator request, CancellationToken ct)
        {
            var enrollment = await _mediator.Send(new GetEnrollmentByIdQuery(request.EnrollmentID), ct);
            if (!enrollment.Success || enrollment.Data is null)
                return RequestResponse.Fail("Enrollment not found", 404);

            var track = await _mediator.Send(new GetByIdTrackQuery(request.newTrackID), ct);
            if (!track.Success || track.Data is null)
                return RequestResponse.Fail("Track not found", 404);

            if (!track.Data.IsActive)
                return RequestResponse.Fail("Track is not active", 400);

            if (track.Data.Id == enrollment.Data.TrackId)
                return RequestResponse.Fail("Already in this track", 400);

            // Capacity check
            var count = await _mediator.Send( new getActiveEnrollmentCountByTrackQuery(request.newTrackID), ct);
            if (count >= track.Data.MaxCapacity)
                return RequestResponse.Fail("Track is full", 400);

            await _uow.ExecuteAsync(async () =>
            {
                await _mediator.Send(
                    new updateEnrollmentTrackCommand(request.EnrollmentID, request.newTrackID), ct);
            });

            return RequestResponse.Ok("Transfer accepted");
        }
    }
}
