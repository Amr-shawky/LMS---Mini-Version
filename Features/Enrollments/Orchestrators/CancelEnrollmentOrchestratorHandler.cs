using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Features.Enrollments.Queries;
using LMS___Mini_Version.Features.Payments.Commands;
using LMS___Mini_Version.Features.Shared;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Orchestrators
{
    /// <summary>
    /// Orchestrator Handler — coordinates the "Cancel an Enrollment" workflow.
    ///
    /// Workflow:
    ///   1. Fetch enrollment               → GetEnrollmentByIdQuery
    ///   2. Validate not already cancelled
    ///   3. Stage status → Cancelled       → StageUpdateEnrollmentStatusCommand  (staged)
    ///   4. Stage payment refund           → StageRefundPaymentCommand            (staged)
    ///   5. COMMIT atomically              → IUnitOfWork.CompleteAsync()
    /// </summary>
    public class CancelEnrollmentOrchestratorHandler
        : IRequestHandler<CancelEnrollmentOrchestratorRequest, RequestResponse<string>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public CancelEnrollmentOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse<string>> Handle(
            CancelEnrollmentOrchestratorRequest request, CancellationToken cancellationToken)
        {
            // Step 1: Fetch the enrollment
            var enrollment = await _mediator
                .Send(new GetEnrollmentByIdQuery(request.EnrollmentId), cancellationToken);

            if (enrollment == null)
            {
                return RequestResponse<string>.Fail(
                    $"Enrollment with ID {request.EnrollmentId} was not found.");
            }

            // Step 2: Validate not already cancelled
            if (enrollment.Status == EnrollmentStatus.Cancelled.ToString())
            {
                return RequestResponse<string>.Fail("This enrollment is already cancelled.");
            }

            // Step 3: Stage status change to Cancelled (NOT saved)
            var statusUpdated = await _mediator
                .Send(new StageUpdateEnrollmentStatusCommand(
                    request.EnrollmentId, EnrollmentStatus.Cancelled), cancellationToken);

            if (!statusUpdated)
            {
                return RequestResponse<string>.Fail("Failed to update enrollment status.");
            }

            // Step 4: Stage payment refund (NOT saved)
            await _mediator
                .Send(new StageRefundPaymentCommand(request.EnrollmentId), cancellationToken);

            // Step 5: ATOMIC COMMIT — cancellation + refund in one transaction
            await _unitOfWork.CompleteAsync();

            return RequestResponse<string>.Success(
                "Cancelled",
                "Enrollment cancelled and payment refunded successfully.");
        }
    }
}
