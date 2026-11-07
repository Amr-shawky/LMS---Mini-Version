using LMS___Mini_Version.Persistence;
using MassTransit;
namespace LMS___Mini_Version.Messaging
{
    public static class MessagingExtensions
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.Registerconsumers();

                x.AddEntityFrameworkOutbox<AppDbContext>(o =>
                {
                    o.QueryDelay = TimeSpan.FromSeconds(1);
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

                    cfg.configureFanout(context);
                });
            });

            return services;
        }
    }
}
