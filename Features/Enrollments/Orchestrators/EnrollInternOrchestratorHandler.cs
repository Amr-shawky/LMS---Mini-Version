using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Features.Interns.Queries;
using LMS___Mini_Version.Features.Payments.Commands;
using LMS___Mini_Version.Features.Shared;
using LMS___Mini_Version.Features.Tracks.Queries;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Orchestrators
{
    /// <summary>
    /// Orchestrator Handler — coordinates the multi-step "Enroll an Intern" workflow.
    /// Injects ONLY IMediator (to dispatch atomic steps) and IUnitOfWork (to commit).
    ///
    /// Workflow:
    ///   1. Validate intern exists       → ValidateInternExistsQuery
    ///   2. Validate track (exists/active)→ GetTrackByIdQuery
    ///   3. Check track capacity         → CheckTrackCapacityQuery
    ///   4. Stage enrollment             → StageEnrollmentCommand          (staged)
    ///   5. COMMIT enrollment            → IUnitOfWork.CompleteAsync()     (gets real ID)
    ///   6. Stage payment (if paid track)→ StagePaymentCommand             (staged)
    ///   7. COMMIT payment               → IUnitOfWork.CompleteAsync()     (atomic)
    /// </summary>
    public class EnrollInternOrchestratorHandler
        : IRequestHandler<EnrollInternOrchestratorRequest, RequestResponse<EnrollmentWithPaymentDto>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public EnrollInternOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse<EnrollmentWithPaymentDto>> Handle(
            EnrollInternOrchestratorRequest request, CancellationToken cancellationToken)
        {
            // Step 1: Validate the intern exists
            var internExists = await _mediator
                .Send(new ValidateInternExistsQuery(request.InternId), cancellationToken);

            if (!internExists)
            {
                return RequestResponse<EnrollmentWithPaymentDto>.Fail(
                    $"Intern with ID {request.InternId} was not found.");
            }

            // Step 2: Validate the track exists and is active
            var track = await _mediator
                .Send(new GetTrackByIdQuery(request.TrackId), cancellationToken);

            if (track == null)
            {
                return RequestResponse<EnrollmentWithPaymentDto>.Fail(
                    $"Track with ID {request.TrackId} was not found.");
            }

            if (!track.IsActive)
            {
                return RequestResponse<EnrollmentWithPaymentDto>.Fail(
                    $"Track '{track.Name}' is not currently active.");
            }

            // Step 3: Check capacity
            var hasCapacity = await _mediator
                .Send(new CheckTrackCapacityQuery(request.TrackId), cancellationToken);

            if (!hasCapacity)
            {
                return RequestResponse<EnrollmentWithPaymentDto>.Fail(
                    $"Track '{track.Name}' has reached its maximum capacity.");
            }

            // Step 4: Stage enrollment (NOT saved yet)
            var enrollment = await _mediator
                .Send(new StageEnrollmentCommand(request.InternId, request.TrackId), cancellationToken);

            // Step 5: COMMIT — enrollment gets a real ID from the database
            await _unitOfWork.CompleteAsync();

            // Step 6: If the track has fees, stage a payment with the real enrollment ID
            PaymentDto? payment = null;
            if (track.Fees > 0)
            {
                var paymentEntity = await _mediator
                    .Send(new StagePaymentCommand(enrollment.Id, track.Fees, PaymentMethod.Cash),
                          cancellationToken);

                // Step 7: COMMIT the payment record
                await _unitOfWork.CompleteAsync();

                payment = paymentEntity.ToDto();
            }

            return RequestResponse<EnrollmentWithPaymentDto>.Success(
                new EnrollmentWithPaymentDto
                {
                    Enrollment = enrollment.ToDto(),
                    Payment = payment
                },
                "Intern enrolled successfully.");
        }
    }
}
