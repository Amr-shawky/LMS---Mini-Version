using LMS___Mini_Version.Persistence;
using MassTransit;

namespace LMS___Mini_Version.Messaging;

public static class MassTransitRegistration
{
    public static IServiceCollection AddEnrollmentMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<RefundEnrollmentTransferredConsumer>();
            x.AddConsumer<TransferNotificationConsumer>();

            x.AddEntityFrameworkOutbox<AppDbContext>(o =>
            {
                o.UseSqlServer();
                o.UseBusOutbox();
            });

            x.UsingRabbitMq((context, cfg) =>
            {

                cfg.Host("localhost", "Elevate", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                // Publish topology uses a fanout exchange; each receive endpoint owns a queue.
                cfg.Publish<EnrollmentTransferred>(p => p.ExchangeType = "fanout");

                cfg.ReceiveEndpoint("enrollment-transfer-refund", e =>
                {
                    e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(2)));
                    e.ConfigureConsumer<RefundEnrollmentTransferredConsumer>(context);
                });
                cfg.ReceiveEndpoint("enrollment-transfer-notification", e =>
                {
                    e.UseMessageRetry(r => r.Immediate(2));
                    e.ConfigureConsumer<TransferNotificationConsumer>(context);
                });
            });
        });
        return services;
    }
}
