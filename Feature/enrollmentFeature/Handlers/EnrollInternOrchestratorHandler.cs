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
            var internResponse = await _mediator.Send(new CheckInternExistsQuery(request.InternId), cancellationToken);
            if (!internResponse.Success || !internResponse.Data)
                return RequestResponse<int>.Fail($"Intern with ID {request.InternId} was not found.", 404);
            

            // Step 2: Validate track exists and is active
            var trackResponse = await _mediator.Send(new GetByIdTrackQuery(request.TrackId), cancellationToken);
            if (!trackResponse.Success || trackResponse.Data == null)
                return RequestResponse<int>.Fail($"Track with ID {request.TrackId} was not found.", 404);
            

            var track = trackResponse.Data;
            if (!track.IsActive)
                return RequestResponse<int>.Fail($"Track '{track.Name}' is not currently active.", 400);
          
            var existingEnrollmentResponse = await _mediator.Send(new GetEnrollmentByInternAndTrackQuery(request.InternId, request.TrackId), cancellationToken);
            if (existingEnrollmentResponse.Success == false)
                return RequestResponse<int>.Fail("Failed to verify existing enrollment.", 500);

            if (existingEnrollmentResponse.Data )
                return RequestResponse<int>.Fail($"Intern with ID {request.InternId} is already enrolled in track '{track.Name}'.", 400);
            
            // Step 3: Check track capacity
            var activeCount = await _mediator.Send(new getActiveEnrollmentCountByTrackQuery(request.TrackId), cancellationToken);
            if (activeCount >= track.MaxCapacity)
                return RequestResponse<int>.Fail($"Track '{track.Name}' has reached its maximum capacity.", 400);
            

            int enrollmentId = 0;

            // Step 4: Atomic creation via UnitOfWork
            await _unitOfWork.ExecuteAsync(async cancellationToken =>
            {
                var enrollmentResponse = await _mediator.Send(new CreateEnrollmentCommand(request.InternId, request.TrackId), cancellationToken);

                if (!enrollmentResponse.Success)
                    throw new InvalidOperationException("Failed to create enrollment.");

                enrollmentId = enrollmentResponse.Data;
                if (track.Fees > 0)
                {
                    var paymentResponse = await _mediator.Send(new CreatePaymentCommand(
                        enrollmentId,
                        track.Fees,
                        PaymentMethod.Cash,
                        PaymentStatus.Pending), cancellationToken);

                    if (!paymentResponse.Success)
                        throw new InvalidOperationException("Failed to create payment.");
                }
            }, cancellationToken);

            return RequestResponse<int>.Created(enrollmentId);
        }
    }
}
