using LMS___Mini_Version.contracts;
using LMS___Mini_Version.Feature.consumers;
using MassTransit;

namespace LMS___Mini_Version.Messaging
{
    public static class RabbitMqTopologyExtension
    {
        public static void configurefanout(this IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext context)
        {
            cfg.Message<trackcreatedmessage>(m =>
            {
                m.SetEntityName("track-exchange");
            });
            cfg.ReceiveEndpoint("track-created-queue", e =>
            {

                e.ConfigureConsumer<trackcreatedConsumer>(context);
            });
            cfg.ReceiveEndpoint("notification-queue", e =>
            {
                e.ConfigureConsumer<NotificationConsumer>(context);
            });

            cfg.Message<PaymentConfirmedMessage>(p =>
            {
                p.SetEntityName("payment-confirmed-message");
            });

            cfg.ReceiveEndpoint("Payment-confirmed-queue", p => {

                p.ConfigureConsumer<PaymentConfirmedConsumer>(context);
            });
        }

        public static void configureDirect(this IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext context)
        {
            cfg.Message<PaymentProcessedMessage>(p =>
            {

                p.SetEntityName("payment-exchange");

            });
            cfg.Publish<PaymentProcessedMessage>(p =>
            {

                p.ExchangeType = "direct";

            });
            cfg.ReceiveEndpoint("cash-queue", c =>
            {

                c.ConfigureConsumeTopology = false;

                c.Bind("payment-exchange", p =>
                {

                    p.ExchangeType = "direct";
                    p.RoutingKey = "cash";
                });

                c.ConfigureConsumer<CashPaymentConsumer>(context);
            });
            cfg.ReceiveEndpoint("visa-queue", v =>
            {
                v.ConfigureConsumeTopology = false;
                v.Bind("payment-exchange", e =>
                {

                    e.ExchangeType = "direct";
                    e.RoutingKey = "visa";

                });
                v.UseMessageRetry(r =>
                {
                    r.Interval(3, TimeSpan.FromSeconds(5));
                });
                v.ConfigureConsumer<VisaPaymentConsumer>(context);
            });
        }
    }
}
