using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.CancelEnrollment;
using LMS___Mini_Version.Feature.enrollmentFeature.TransferEnrollment;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Consumers
{
    public class RefundConsumer : IConsumer<EnrollmentTransferredEvent>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RefundConsumer> _logger;

        public RefundConsumer(IMediator mediator, IUnitOfWork unitOfWork, ILogger<RefundConsumer> logger)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<EnrollmentTransferredEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation("Processing refund evaluation for transferred enrollment ID: {EnrollmentId}", message.EnrollmentId);

            await _unitOfWork.ExecuteAsync(async () =>
            {
                // Step 1: Execute refund Command for the original payment
                var refundResponse = await _mediator.Send(new refundPaymentCommand(message.EnrollmentId));

                // If payment was not eligible, no refund is processed
                if (!refundResponse.Success || refundResponse.Data == null || !refundResponse.Data.IsEligible)
                {
                    _logger.LogInformation("Payment for enrollment ID {EnrollmentId} is not eligible for refund.", message.EnrollmentId);
                    return;
                }

                // Step 2: Establish savepoint between payment refund and audit history creation
                const string savepointName = "BeforeRefundHistory";
                await _unitOfWork.AddSavePointAsync(savepointName);
                _logger.LogInformation("Established savepoint '{Savepoint}' for enrollment ID {EnrollmentId}", savepointName, message.EnrollmentId);

                try
                {
                    // Step 3: Record RefundHistory
                    var historyResponse = await _mediator.Send(new CreateRefundHistoryCommand(
                        refundResponse.Data.PaymentId,
                        message.EnrollmentId,
                        refundResponse.Data.PreviousStatus,
                        PaymentStatus.Refunded
                    ));

                    if (!historyResponse.Success)
                    {
                        throw new InvalidOperationException($"Failed to record refund history for enrollment ID {message.EnrollmentId}");
                    }

                    _logger.LogInformation("Successfully processed refund and refund history for enrollment ID {EnrollmentId}", message.EnrollmentId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to record refund history after refunding payment. Rolling back to savepoint '{Savepoint}' and propagating failure for MassTransit retry.", savepointName);
                    await _unitOfWork.RollbackToSavePointAsync(savepointName);
                    throw; // Re-throw so transaction completely rolls back and MassTransit retry policy triggers
                }
            });
        }
    }
}

