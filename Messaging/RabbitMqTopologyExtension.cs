using LMS___Mini_Version.contracts;
using LMS___Mini_Version.Feature.EnrollmentFeature.Consumers;
using MassTransit;

namespace LMS___Mini_Version.Messaging
{
    public static class RabbitMqTopologyExtension
    {
        public static void configureFanout(this IRabbitMqBusFactoryConfigurator cfg , IBusRegistrationContext context)
        {
            #region Configure Fanout Exchange (Transfer Enrollment Refund & Notification)

            //Set the entity name for the EnrollmentTransferredMessage to use a fanout exchange
            cfg.Message<EnrollmentTransferredMessage>(m =>
            {
                m.SetEntityName("enrollment-transferred-exchange");
            });

            //Bind the RefundConsumer to the fanout exchange
            cfg.ReceiveEndpoint("refund-enrollment-queue", e =>
            {
                e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(2)));
                e.ConfigureConsumer<RefundConsumer>(context);
            });

            cfg.ReceiveEndpoint("notification-enrollment-queue", e =>
            {
                e.ConfigureConsumer<NotificationConsumer>(context);
            });

            #endregion
        }
    }
}
