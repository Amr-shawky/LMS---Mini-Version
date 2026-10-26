using LMS___Mini_Version.Feature.paymentFeature.Consumers;
using LMS___Mini_Version.Feature.paymentFeature.Events;
using LMS___Mini_Version.Feature.Tracks.Consumers;
using LMS___Mini_Version.Feature.Tracks.Events;
using MassTransit;

namespace LMS___Mini_Version.Infrastructure.Messaging
{
    public static class RabbitMqTopologyExtensions
    {
        public static void ConfigureFanoutEndPoint(this IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext context)
        {   
            cfg.Message<TrackCreatedMessage>(m => m.SetEntityName("track-created-exchange"));
            cfg.ReceiveEndpoint("track-created-queue", e => e.ConfigureConsumer<TrackCreatedConsumer>(context));
            cfg.ReceiveEndpoint("add-notification-queue", e => e.ConfigureConsumer<AddConsumerNotification>(context));
            //PaymentConfirmed
            cfg.Message<PaymentConfirmedMessage>(p => p.SetEntityName("Payment-Confirmed-Exchange"));
            cfg.ReceiveEndpoint("Payment-Confirmed-Queue", p => p.ConfigureConsumer<PaymentConfirmedConsumer>(context));
        }
        public static void ConfigureDirectEndPoint(this IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext context)
        {
            cfg.Message<PaymentProcessedMessage>(p => p.SetEntityName("payment-exchange"));
            cfg.Publish<PaymentProcessedMessage>(p => p.ExchangeType = "direct");
            //visa 
            cfg.ReceiveEndpoint("visa-payment-queue", v =>
            {
                v.ConfigureConsumeTopology = false;
                v.Bind("payment-exchange", b =>
                {
                    b.ExchangeType = "direct";
                    b.RoutingKey = "visa-payment";
                });
                v.ConfigureConsumer<VisaPaymentConsumer>(context);
            });
            //Cash
            cfg.ReceiveEndpoint("cash-payment-queue", c =>
            {
                c.ConfigureConsumeTopology = false;
                c.Bind("payment-exchange", c =>
                {
                    c.ExchangeType = "direct";
                    c.RoutingKey = "cash-payment";
                });
                c.UseMessageRetry(c =>
                {
                    c.Interval(4,TimeSpan.FromSeconds(10));
                });
                c.UseInMemoryOutbox(context);
                c.ConfigureConsumer<CashPaymentConsumer>(context);
            });
        }
    }
}
