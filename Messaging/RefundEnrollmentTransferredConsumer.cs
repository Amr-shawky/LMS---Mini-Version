using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using MassTransit;
using MediatR;

namespace LMS___Mini_Version.Messaging;

public sealed class RefundEnrollmentTransferredConsumer(
    IMediator mediator, IUnitOfWork uow, ILogger<RefundEnrollmentTransferredConsumer> logger) : IConsumer<EnrollmentTransferred>
{
    public async Task Consume(ConsumeContext<EnrollmentTransferred> context)
    {
        await uow.ExecuteAsync(async () =>
        {
            var refund = await mediator.Send(new refundPaymentCommand(context.Message.EnrollmentId), context.CancellationToken);
            if (!refund.Success)
                throw new InvalidOperationException(refund.Message);
            if (refund.Data == 0)
            {
                logger.LogInformation("No eligible payment to refund for transfer {TransferId}", context.Message.TransferId);
                return;
            }

            await uow.AddSavePointAsync("BeforeRefundHistory");
            try
            {
                var history = await mediator.Send(new CreateRefundHistoryCommand(refund.Data, context.Message.EnrollmentId), context.CancellationToken);
                if (!history.Success)
                    throw new InvalidOperationException(history.Message);
            }
            catch
            {
                await uow.RollbackToSavePointAsync("BeforeRefundHistory");
                throw; // mandatory history failed: outer UOF rolls back and MassTransit retries
            }
        });
    }
}
