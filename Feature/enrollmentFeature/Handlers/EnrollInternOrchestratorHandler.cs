using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using LMS___Mini_Version.Feature.internFeature.Queries;
using LMS___Mini_Version.Feature.paymentFeature.Commands;
using LMS___Mini_Version.Feature.Tracks.Query;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public class EnrollInternOrchestratorHandler : IRequestHandler<EnrollInternOrchestrator, RequestResponse<EnrollmentDto>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public EnrollInternOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse<EnrollmentDto>> Handle(EnrollInternOrchestrator request, CancellationToken cancellationToken)
        {
            // Step 1: Validate intern exists
            var internResponse = await _mediator.Send(new GetInternByIdQuery(request.InternId), cancellationToken);
            if (!internResponse.Success || internResponse.Data == null)
            {
                return RequestResponse<EnrollmentDto>.Fail($"Intern with ID {request.InternId} was not found.", 404);
            }

            // Step 2: Validate track exists and is active
            var trackResponse = await _mediator.Send(new GetByIdTrackQuery(request.TrackId), cancellationToken);
            if (!trackResponse.Success || trackResponse.Data == null)
            {
                return RequestResponse<EnrollmentDto>.Fail($"Track with ID {request.TrackId} was not found.", 404);
            }

            var track = trackResponse.Data;
            if (!track.IsActive)
            {
                return RequestResponse<EnrollmentDto>.Fail($"Track '{track.Name}' is not currently active.", 400);
            }

            // Step 3: Check track capacity
            var activeCount = await _mediator.Send(new getActiveEnrollmentCountByTrackQuery(request.TrackId), cancellationToken);
            if (activeCount >= track.MaxCapacity)
            {
                return RequestResponse<EnrollmentDto>.Fail($"Track '{track.Name}' has reached its maximum capacity.", 400);
            }

            RequestResponse<EnrollmentDto> enrollmentResult = null!;

            // Step 4: Atomic creation via UnitOfWork
            await _unitOfWork.ExecuteAsync(async () =>
            {
                enrollmentResult = await _mediator.Send(new CreateEnrollmentCommand(request.InternId, request.TrackId), cancellationToken);

                if (track.Fees > 0)
                {
                    await _mediator.Send(new CreatePaymentCommand(
                        enrollmentResult.Data!.Id,
                        track.Fees,
                        PaymentMethod.Cash,
                        PaymentStatus.Pending), cancellationToken);
                }
            });

            return enrollmentResult;
        }
    }
}
