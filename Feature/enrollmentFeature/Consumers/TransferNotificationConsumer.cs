using LMS___Mini_Version.Feature.enrollmentFeature.TransferEnrollment;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Consumers
{
    public class TransferNotificationConsumer : IConsumer<EnrollmentTransferredEvent>
    {
        private readonly ILogger<TransferNotificationConsumer> _logger;

        public TransferNotificationConsumer(ILogger<TransferNotificationConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<EnrollmentTransferredEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation(
                "[Notification] Intern ID {InternId} successfully transferred Enrollment ID {EnrollmentId} from Track ID {OldTrackId} to Track ID {NewTrackId} at {TransferredAt}.",
                message.InternId,
                message.EnrollmentId,
                message.OldTrackId,
                message.NewTrackId,
                message.TransferredAt
            );

            return Task.CompletedTask;
        }
    }
}

