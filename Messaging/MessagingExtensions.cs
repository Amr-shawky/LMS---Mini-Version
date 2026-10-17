using LMS___Mini_Version.contracts;
using LMS___Mini_Version.Feature.consumers;
using LMS___Mini_Version.Persistence;
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
                    x.AddEntityFrameworkOutbox<AppDbContext>(o =>
                    {
                        o.UseSqlServer();
                    });
                    cfg.configurefanout(context);
                    cfg.configureDirect(context);
                });
            });
            return services;
        }

    }
}
