using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    /// <summary>
    /// Orchestrator that cancels an enrollment and performs a synchronous refund when eligible.
    ///
    /// Flow inside a single UOF transaction (R10, R15):
    ///   1. Verify the enrollment exists and is not already cancelled.
    ///   2. Update enrollment status to Cancelled.
    ///   3. Attempt refund via shared refundPaymentCommand (no-op when payment is missing/not Completed).
    ///   4. If a refund happened, record RefundHistory (R23, R24).
    ///
    /// Using the shared refundPaymentCommand means the eligibility rule lives in one place,
    /// reused by both Cancel and the async Transfer consumer (R15, D4).
    /// </summary>
    public class cancelEnrollmentOrchestratorHandler : IRequestHandler<cancelEnrollmentOrchestrator, RequestResponse>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public cancelEnrollmentOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse> Handle(cancelEnrollmentOrchestrator request, CancellationToken cancellationToken)
        {
            // Step 1: verify enrollment exists
            var enrollmentResponse = await _mediator.Send(new GetEnrollmentByIdQuery(request.enrollmentID), cancellationToken);

            if (!enrollmentResponse.Success || enrollmentResponse.Data == null)
                return RequestResponse.Fail($"Enrollment {request.enrollmentID} not found.", 404);

            if (enrollmentResponse.Data.Status == EnrollmentStatus.Cancelled)
                return RequestResponse.Fail("Enrollment is already cancelled.", 400);

            // Step 2 & 3: update status and attempt refund inside one atomic transaction
            await _unitOfWork.ExecuteAsync(async () =>
            {
                await _mediator.Send(
                    new updateEnrollmentStatusCommand(request.enrollmentID, EnrollmentStatus.Cancelled),
                    cancellationToken);

                // refundPaymentCommand returns the PaymentId (>0) if a refund was applied, 0 if not eligible.
                var refundResult = await _mediator.Send(
                    new refundPaymentCommand(request.enrollmentID), cancellationToken);

                // Step 4: record history only when a refund actually happened
                if (refundResult.Data > 0)
                {
                    await _mediator.Send(new CreateRefundHistoryCommand(
                        EnrollmentId: request.enrollmentID,
                        PaymentId: refundResult.Data,
                        PreviousStatus: PaymentStatus.Completed,
                        NewStatus: PaymentStatus.Refunded), cancellationToken);
                }
            });

            return RequestResponse.Ok("Enrollment cancelled successfully.");
        }
    }
}