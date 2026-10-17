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
                x.RegisterConsumers();

                x.UsingRabbitMq((context, cfg) =>
                {

                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.UseInMemoryOutbox(context);
                    cfg.configurefanout(context);
                    cfg.configureDirect(context);
                });
            });
            return services;
        }

    }
}
