using LMS___Mini_Version.Feature.enrollmentFeature.Messaging.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Messaging.Consumers
{
    /// <summary>
    /// Observes EnrollmentTransferred events and logs identifying information (R20).
    ///
    /// This consumer does NOT share a queue with RefundConsumer (R18).
    /// Each gets its own dedicated receive queue bound to the same fanout exchange,
    /// so every transfer event is delivered independently to both consumers.
    ///
    /// No email/SMS provider is used -- logging is the only output (R20).
    /// </summary>
    public class NotificationConsumer : IConsumer<EnrollmentTransferred>
    {
        private readonly ILogger<NotificationConsumer> _logger;

        public NotificationConsumer(ILogger<NotificationConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<EnrollmentTransferred> context)
        {
            var msg = context.Message;

            _logger.LogInformation(
                "NOTIFICATION -- EnrollmentTransferred received. " +
                "MessageId={MessageId}, EnrollmentId={EnrollmentId}, InternId={InternId}, " +
                "OldTrackId={OldTrackId}, NewTrackId={NewTrackId}, OccurredAt={OccurredAt}",
                context.MessageId,
                msg.EnrollmentId,
                msg.InternId,
                msg.OldTrackId,
                msg.NewTrackId,
                msg.OccurredAtUtc);

            return Task.CompletedTask;
        }
    }
}