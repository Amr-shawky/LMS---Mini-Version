using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using LMS___Mini_Version.Feature.enrollmentFeature.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Consumers
{
    public sealed class TransferRefundConsumer : MassTransit.IConsumer<EnrollmentTransferred>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public TransferRefundConsumer(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(MassTransit.ConsumeContext<EnrollmentTransferred> context)
        {
            await _unitOfWork.ExecuteAsync(async () =>
            {
                var result = await _mediator.Send(
                    new refundPaymentCommand(context.Message.EnrollmentId), context.CancellationToken);
                if (!result.Success)
                    throw new InvalidOperationException(result.Message);

                await _unitOfWork.AddSavePointAsync("refund-complete");
            });
        }
    }

    public sealed class TransferNotificationConsumer : MassTransit.IConsumer<EnrollmentTransferred>
    {
        private readonly ILogger<TransferNotificationConsumer> _logger;

        public TransferNotificationConsumer(ILogger<TransferNotificationConsumer> logger) => _logger = logger;

        public Task Consume(MassTransit.ConsumeContext<EnrollmentTransferred> context)
        {
            _logger.LogInformation("Enrollment {EnrollmentId} transferred from track {OriginalTrackId} to track {DestinationTrackId}.",
                context.Message.EnrollmentId, context.Message.OriginalTrackId, context.Message.DestinationTrackId);
            return Task.CompletedTask;
        }
    }
}