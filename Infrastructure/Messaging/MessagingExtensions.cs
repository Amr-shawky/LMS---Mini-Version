using MassTransit;

namespace LMS___Mini_Version.Infrastructure.Messaging
{
    public static class MessagingExtensions
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.RegisterConsumers();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                   cfg.ConfigureFanoutEndPoint(context);
                   cfg.ConfigureDirectEndPoint(context);
                });
            });
            return services;
        }
        
    }
}
