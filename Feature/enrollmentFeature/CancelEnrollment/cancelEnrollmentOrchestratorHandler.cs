using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.GetEnrollmentById;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.CancelEnrollment
{
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
            // Step 1: Check if enrollment exists
            var enrollmentResponse = await _mediator.Send(new GetEnrollmentByIdQuery(request.enrollmentID), cancellationToken);

            if (!enrollmentResponse.Success || enrollmentResponse.Data == null)
            {
                return RequestResponse.Fail($"Enrollment not found {request.enrollmentID}", 404);
            }

            var enrollment = enrollmentResponse.Data;

            if (enrollment.Status == EnrollmentStatus.Cancelled)
            {
                return RequestResponse.Fail("Enrollment is already cancelled", 400);
            }

            // Step 2: Execute updates atomically within UnitOfWork transaction
            await _unitOfWork.ExecuteAsync(async () =>
            {
                // 2a. Update enrollment status to Cancelled
                await _mediator.Send(new updateEnrollmentStatusCommand(request.enrollmentID, EnrollmentStatus.Cancelled), cancellationToken);

                // 2b. Process refund if eligible (PaymentStatus == Completed)
                var refundResponse = await _mediator.Send(new refundPaymentCommand(request.enrollmentID), cancellationToken);

                // 2c. Record RefundHistory if an eligible payment was refunded
                if (refundResponse.Success && refundResponse.Data != null && refundResponse.Data.IsEligible)
                {
                    await _mediator.Send(new CreateRefundHistoryCommand(
                        refundResponse.Data.PaymentId,
                        request.enrollmentID,
                        refundResponse.Data.PreviousStatus,
                        PaymentStatus.Refunded), cancellationToken);
                }
            });

            return RequestResponse.Ok("Enrollment cancelled successfully");
        }
    }
}
