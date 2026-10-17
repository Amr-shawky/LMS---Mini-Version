using LMS___Mini_Version.contracts;
using LMS___Mini_Version.Feature.consumers;
using MassTransit;

namespace LMS___Mini_Version.Messaging
{
    public static class MessagingExtensions
    {
        public static IServiceCollection Addmessaging(this IServiceCollection services)
        {

            services.AddMassTransit((x) =>
            {
                RegisterConsumers(x);

                x.UsingRabbitMq((context, cfg) =>
                {

                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.UseInMemoryOutbox(context);

                    configurefanout(context,cfg);
                    configureDirect(context, cfg);
                });
            });
            return services;
        }

        private static void RegisterConsumers(IBusRegistrationConfigurator registration)
        {
            registration.AddConsumer<trackcreatedConsumer>();
            registration.AddConsumer<NotificationConsumer>();

            //new 
            registration.AddConsumer<VisaPaymentConsumer>();
            registration.AddConsumer<CashPaymentConsumer>();

        }


        private static void configurefanout(IBusRegistrationContext context,IRabbitMqBusFactoryConfigurator cfg)
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

        }

        private static void configureDirect(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator cfg)
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

                    r.Interval(3, TimeSpan.FromSeconds(2));
                });
                v.ConfigureConsumer<VisaPaymentConsumer>(context);
            });
        }
    }
}
