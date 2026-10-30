using MassTransit;

namespace LMS___Mini_Version.Messaging;

public sealed class TransferNotificationConsumer(ILogger<TransferNotificationConsumer> logger) : IConsumer<EnrollmentTransferred>
{
    public Task Consume(ConsumeContext<EnrollmentTransferred> context)
    {
        var m = context.Message;
        logger.LogInformation("Enrollment {EnrollmentId} transferred {Previous}->{Next}, transfer {TransferId}",
            m.EnrollmentId, m.PreviousTrackId, m.NewTrackId, m.TransferId);
        return Task.CompletedTask;
    }
}
