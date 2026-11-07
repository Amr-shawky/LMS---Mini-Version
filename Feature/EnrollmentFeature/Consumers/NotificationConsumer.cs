using LMS___Mini_Version.contracts;
using MassTransit;

namespace LMS___Mini_Version.Feature.EnrollmentFeature.Consumers
{
    public class NotificationConsumer : IConsumer<EnrollmentTransferredMessage>
    {
        private readonly ILogger<NotificationConsumer> _logger;

        public NotificationConsumer(ILogger<NotificationConsumer> logger)
        {
            _logger = logger;
        }
        public Task Consume(ConsumeContext<EnrollmentTransferredMessage> context)
        {
            var msg = context.Message;
            _logger.LogInformation(
                "NOTIFICATION: Enrollment {EnrollmentId} transferred from Track {OldTrackId} to Track {NewTrackId} at {OccurredAt}",
                msg.EnrollmentId, msg.OldTrackId, msg.NewTrackId, msg.OccurredAt);
            
            return Task.CompletedTask;
        }
    }
}
