using LMS___Mini_Version.Feature.paymentFeature.Consumers;
using LMS___Mini_Version.Feature.paymentFeature.Enents;
using LMS___Mini_Version.Feature.Tracks.Consumers;
using LMS___Mini_Version.Feature.Tracks.Events;
using MassTransit;

namespace LMS___Mini_Version.Infrastructure.Messaging
{
    public static class MessagingExtensions
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<TrackCreatedConsumer>();
                x.AddConsumer<AddConsumerNotification>();
                x.AddConsumer<VisaPaymentConsumer>();
                x.AddConsumer<CashPaymentConsumer>();
                x.UsingRabbitMq((Context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.Message<TrackCreatedMessage>(m => m.SetEntityName("track-created-exchange"));
                    cfg.ReceiveEndpoint("track-created-queue", e => e.ConfigureConsumer<TrackCreatedConsumer>(Context));
                    cfg.ReceiveEndpoint("add-notification-queue", e => e.ConfigureConsumer<AddConsumerNotification>(Context));

                    cfg.Message<PaymentProcessedMessage>(p => p.SetEntityName("payment-exchange"));
                    cfg.Publish<PaymentProcessedMessage>(p => p.ExchangeType = "direct");
                    // VisaPayment 
                    cfg.ReceiveEndpoint("visa-payment-queue", v =>
                    {
                        v.ConfigureConsumeTopology = false;
                        v.Bind("payment-exchange", v =>
                        {
                            v.ExchangeType = "direct";
                            v.RoutingKey = "visa-payment";
                        });
                        v.ConfigureConsumer<VisaPaymentConsumer>(Context);
                    });
                    // cash payment
                  
                    cfg.ReceiveEndpoint("cash-payment-queue", c =>
                    {
                        c.ConfigureConsumeTopology = false;
                        c.Bind("payment-exchange", c =>
                        {
                            c.ExchangeType = "direct";  
                            c.RoutingKey = "cash-payment";
                        });
                        c.ConfigureConsumer<CashPaymentConsumer>(Context);
                    });

                    //==============================================
                    //// Configure the endpoints for all consumers   ||
                    //     cfg.ConfigureEndpoints(Context);          ||  
                    //===============================================
                });
            });
            return services;
        }
    }
}
