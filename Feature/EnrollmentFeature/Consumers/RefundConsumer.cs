using LMS___Mini_Version.contracts;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using LMS___Mini_Version.Feature.EnrollmentFeature.Commands;
using MassTransit;
using MediatR;

namespace LMS___Mini_Version.Feature.EnrollmentFeature.Consumers
{
    public class RefundConsumer : IConsumer<EnrollmentTransferredMessage>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RefundConsumer> _logger;

        public RefundConsumer(
            IMediator mediator,
            IUnitOfWork unitOfWork,
            ILogger<RefundConsumer> logger)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<EnrollmentTransferredMessage> context)
        {
            #region Retry Message Error :
            //_logger.LogWarning("Attempt {Attempt}", context.GetRetryAttempt());

            //if (context.GetRetryAttempt() < 2)
            //    throw new InvalidOperationException("Simulated transient failure");
            #endregion

            //1.get the message from the context
            var msg = context.Message;

            //2.log the received message
            _logger.LogInformation("RefundConsumer received transfer of enrollment {EnrollmentId}", msg.EnrollmentId);

            //3. Execute the refund payment command within a unit of work
            await _unitOfWork.ExecuteAsync(async () =>
            {
                var refund = await _mediator.Send(new refundPaymentCommand(msg.EnrollmentId), (context.CancellationToken));

                var paymentId = refund.Data;
                if (!refund.Success || paymentId == 0)
                {
                    _logger.LogInformation("No eligible refund for enrollment {Id}", msg.EnrollmentId);
                    return; //No refund No history
                }
                //Add SavePoint for the history :
                await _unitOfWork.AddSavePointAsync("AfterRefund");
                try
                {
                    var history = await _mediator.Send(new CreateRefundHistoryCommand(paymentId , msg.EnrollmentId,PaymentStatus.Completed,PaymentStatus.Refunded));
                    if (!history.Success)
                        throw new InvalidOperationException("Failed to record refund history.");
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "History failed for enrollment {Id}", msg.EnrollmentId);
                    await _unitOfWork.RollbackToSavePointAsync("AfterRefund");
                    throw; 
                }
            });
        }
    }
}
