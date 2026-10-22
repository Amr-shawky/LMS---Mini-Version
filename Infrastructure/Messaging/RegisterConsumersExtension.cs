using LMS___Mini_Version.Feature.paymentFeature.Consumers;
using LMS___Mini_Version.Feature.Tracks.Consumers;
using MassTransit;

namespace LMS___Mini_Version.Infrastructure.Messaging
{
    public static class RegisterConsumersExtension
    {
        public static void RegisterConsumers(this IBusRegistrationConfigurator register)
        {
            register.AddConsumer<TrackCreatedConsumer>();
            register.AddConsumer<AddConsumerNotification>();
            register.AddConsumer<VisaPaymentConsumer>();
            register.AddConsumer<CashPaymentConsumer>();
            register.AddConsumer<PaymentConfirmedConsumer>();
        }

    }
}
