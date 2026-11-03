using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using LMS___Mini_Version.Messages;
using MassTransit;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Consumers
{
    public class EnrollmentTransferredRefundConsumer : IConsumer<EnrollmentTransferred>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public EnrollmentTransferredRefundConsumer(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<EnrollmentTransferred> context)
        {
            var message = context.Message;

            await _unitOfWork.ExecuteAsync(async ct =>
            {
                // CHANGE: Create a savepoint before starting the refund workflow.
                await _unitOfWork.AddSavePointAsync("BeforeRefund", ct);

                var refundResponse = await _mediator.Send(new refundPaymentCommand(message.PreviousEnrollmentId), ct);

                if (!refundResponse.Success)
                    throw new InvalidOperationException(refundResponse.Message);

                if (!refundResponse.Data.HasValue)
                    return;

                var historyResponse = await _mediator.Send(new CreateRefundedPaymentHistoryCommand(refundResponse.Data.Value,
                        message.PreviousEnrollmentId),
                    ct);

                if (!historyResponse.Success)
                {
                    // CHANGE: Roll back the refund if creating its history fails.
                    await _unitOfWork.RollbackToSavePointAsync("BeforeRefund", ct);

                    throw new InvalidOperationException(historyResponse.Message);
                }

            }, context.CancellationToken);
        }
    }
}

