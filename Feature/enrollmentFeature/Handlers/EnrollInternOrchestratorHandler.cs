using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using LMS___Mini_Version.Feature.internFeature.Queries;
using LMS___Mini_Version.Feature.paymentFeature.Commands;
using LMS___Mini_Version.Feature.Tracks.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public class EnrollInternOrchestratorHandler : IRequestHandler<EnrollInternOrchestrator, RequestResponse<int>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public EnrollInternOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse<int>> Handle(EnrollInternOrchestrator request, CancellationToken cancellationToken)
        {
            // Step 1: Validate intern exists
            var internResponse = await _mediator.Send(new GetInternByIdQuery(request.InternId), cancellationToken);
            if (!internResponse.Success || internResponse.Data == null)
            {
                return RequestResponse<int>.Fail($"Intern with ID {request.InternId} was not found.", 404);
            }

            // Step 2: Validate track exists and is active
            var trackResponse = await _mediator.Send(new GetByIdTrackQuery(request.TrackId), cancellationToken);
            if (!trackResponse.Success || trackResponse.Data == null)
            {
                return RequestResponse<int>.Fail($"Track with ID {request.TrackId} was not found.", 404);
            }

            var track = trackResponse.Data;
            if (!track.IsActive)
            {
                return RequestResponse<int>.Fail($"Track '{track.Name}' is not currently active.", 400);
            }

            // Step 3: Check for duplicate enrollment — intern already has an active enrollment on the same track
            var hasActiveEnrollment = await _mediator.Send(
                new GetEnrollmentsByInternQuery(request.InternId, 1, int.MaxValue), cancellationToken);

            if (hasActiveEnrollment.Success && hasActiveEnrollment.Data != null)
            {
                var alreadyEnrolled = hasActiveEnrollment.Data.Items
                    .Any(e => e.TrackId == request.TrackId && e.Status == EnrollmentStatus.Active);

                if (alreadyEnrolled)
                {
                    return RequestResponse<int>.Fail(
                        $"Intern {request.InternId} is already actively enrolled in track '{track.Name}'.", 400);
                }
            }

            // Step 4: Check track capacity
            var activeCount = await _mediator.Send(new getActiveEnrollmentCountByTrackQuery(request.TrackId), cancellationToken);
            if (activeCount >= track.MaxCapacity)
            {
                return RequestResponse<int>.Fail($"Track '{track.Name}' has reached its maximum capacity.", 400);
            }

            int enrollmentId = 0;

            // Step 5: Atomic creation via UnitOfWork — enrollment + payment created together
            await _unitOfWork.ExecuteAsync(async () =>
            {
                var enrollmentResult = await _mediator.Send(
                    new CreateEnrollmentCommand(request.InternId, request.TrackId), cancellationToken);

                enrollmentId = enrollmentResult.Data;

                if (track.Fees > 0)
                {
                    await _mediator.Send(new CreatePaymentCommand(
                        enrollmentId,
                        track.Fees,
                        PaymentMethod.Cash,
                        PaymentStatus.Pending), cancellationToken);
                }
            });

            return RequestResponse<int>.Created(enrollmentId, "Enrollment created successfully.");
        }
    }
}
