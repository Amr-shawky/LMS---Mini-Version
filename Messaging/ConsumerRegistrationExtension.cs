using LMS___Mini_Version.Feature.consumers;
using MassTransit;
using MassTransit.Configuration;

namespace LMS___Mini_Version.Messaging
{
    public static class ConsumerRegistrationExtension
    {
        public static void RegisterConsumers(this IBusRegistrationConfigurator Registration)
        {
            Registration.AddConsumer<trackcreatedConsumer>();
            Registration.AddConsumer<NotificationConsumer>();

            //new 
            Registration.AddConsumer<VisaPaymentConsumer>();
            Registration.AddConsumer<CashPaymentConsumer>();
            Registration.AddConsumer<PaymentConfirmedConsumer>();
        }
    }
}
