using LMS___Mini_Version.Messages;
using MassTransit;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Consumers
{
    public class EnrollmentTransferredNotificationConsumer : IConsumer<EnrollmentTransferred>
    {
        private readonly ILogger<EnrollmentTransferredNotificationConsumer> _logger;

        public EnrollmentTransferredNotificationConsumer(ILogger<EnrollmentTransferredNotificationConsumer> logger) =>
            _logger = logger;

        public Task Consume(ConsumeContext<EnrollmentTransferred> context)
        {
            var message = context.Message;

            _logger.LogInformation(
                "Enrollment {EnrollmentId} transferred from Track {PreviousTrackId} to Track {NewTrackId} for Intern {InternId}",
                message.PreviousEnrollmentId,
                message.PreviousTrackId,
                message.NewTrackId,
                message.InternId);

            return Task.CompletedTask;
        }
    }
}
