using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using LMS___Mini_Version.Feature.enrollmentFeature.Messaging.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Messaging.Consumers
{
    /// <summary>
    /// Processes EnrollmentTransferred events and applies a refund when eligible.
    ///
    /// Why does this consumer use IMediator (R19)?
    ///   It delegates actual database work to existing MediatR commands, keeping the consumer
    ///   thin. This also ensures the same eligibility rule (refundPaymentCommand) used by
    ///   Cancel is reused here (D4, R15).
    ///
    /// Savepoint pattern (R25, D7):
    ///   Inside ONE UOF transaction:
    ///     1. Dispatch refundPaymentCommand  (updates payment status if eligible)
    ///     2. AddSavePointAsync("after-refund")
    ///     3. Dispatch CreateRefundHistoryCommand
    ///        -- if this throws: RollbackToSavePointAsync then RETHROW
    ///           (whole transaction rolls back, MassTransit retries the consumer)
    ///
    /// Purpose of the savepoint:
    ///   SQL savepoints let us undo only the history write, not the earlier refund write, before
    ///   we decide to abort. However, because we then RETHROW, the UOF outer catch rolls back
    ///   the entire transaction anyway. The savepoint therefore gives us a clean rollback point
    ///   that we can target precisely, ensuring no partial state is committed when the history
    ///   step fails. Rolling back to the savepoint and swallowing the error is NOT done (R25).
    ///
    /// Retry (R21): configured on the receive endpoint in Program.cs.
    /// Failure injection (D6): controlled by Messaging:Testing:RefundConsumerFailures in config.
    /// </summary>
    public class RefundConsumer : IConsumer<EnrollmentTransferred>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RefundConsumer> _logger;
        private readonly int _failuresRemaining;

        public RefundConsumer(IMediator mediator, IUnitOfWork unitOfWork, ILogger<RefundConsumer> logger,
            IConfiguration configuration)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _logger = logger;

            // Read the failure injection count from config (default 0 = no failures injected).
            _failuresRemaining = configuration.GetValue<int>("Messaging:Testing:RefundConsumerFailures", 0);
        }

        public async Task Consume(ConsumeContext<EnrollmentTransferred> context)
        {
            var msg = context.Message;
            _logger.LogInformation(
                "RefundConsumer: processing EnrollmentTransferred for EnrollmentId={EnrollmentId}, " +
                "InternId={InternId}, OldTrackId={OldTrackId}, NewTrackId={NewTrackId}",
                msg.EnrollmentId, msg.InternId, msg.OldTrackId, msg.NewTrackId);

            // Controlled failure injection for demonstrating retry behaviour (D6).
            // Default is 0 -- this block is never entered in a normal run.
            if (_failuresRemaining > 0)
            {
                _logger.LogWarning("RefundConsumer: injecting transient failure (remaining={Count})", _failuresRemaining);
                throw new InvalidOperationException("Injected transient failure for retry demonstration.");
            }

            await _unitOfWork.ExecuteAsync(async () =>
            {
                // Step 1: Attempt refund (no-op when payment is not Completed)
                var refundResult = await _mediator.Send(
                    new refundPaymentCommand(msg.EnrollmentId), context.CancellationToken);

                int paymentId = refundResult.Data;

                // Step 2: Add a savepoint after the refund write, before the history write.
                // If the history step fails, we roll back to here (undoing the history attempt)
                // and then rethrow so the UOF transaction also rolls back fully, and MassTransit retries.
                await _unitOfWork.AddSavePointAsync("after-refund");

                // Step 3: Record history only when a refund actually happened
                if (paymentId > 0)
                {
                    try
                    {
                        await _mediator.Send(new CreateRefundHistoryCommand(
                            EnrollmentId: msg.EnrollmentId,
                            PaymentId: paymentId,
                            PreviousStatus: PaymentStatus.Completed,
                            NewStatus: PaymentStatus.Refunded), context.CancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex,
                            "RefundConsumer: history write failed for EnrollmentId={EnrollmentId}; " +
                            "rolling back to savepoint and rethrowing so MassTransit can retry.",
                            msg.EnrollmentId);

                        // Roll back the history write (not the refund -- but the outer UOF catch
                        // will roll back everything since we rethrow below).
                        await _unitOfWork.RollbackToSavePointAsync("after-refund");

                        // Rethrow: do NOT swallow. The UOF rolls back the whole transaction,
                        // and MassTransit retries the consumer. (R25 requirement)
                        throw;
                    }
                }
            });

            _logger.LogInformation(
                "RefundConsumer: completed EnrollmentId={EnrollmentId}", msg.EnrollmentId);
        }
    }
}