using LMS___Mini_Version.Feature.EnrollmentFeature.Consumers;
using MassTransit;

namespace LMS___Mini_Version.Messaging
{
    public static class RegisterConsumers
    {
        public static void Registerconsumers(this IBusRegistrationConfigurator x )
        {
            x.AddConsumer<RefundConsumer>();
            x.AddConsumer<NotificationConsumer>();
        }
    }
}
