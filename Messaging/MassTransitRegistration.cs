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
                var username = configuration["RabbitMq:Username"]
                    ?? throw new InvalidOperationException("Set RabbitMq__Username in environment/user secrets");
                var password = configuration["RabbitMq:Password"]
                    ?? throw new InvalidOperationException("Set RabbitMq__Password in environment/user secrets");

                cfg.Host(configuration["RabbitMq:Host"] ?? "localhost", "Elevate", h =>
                {
                    h.Username(username);
                    h.Password(password);
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
