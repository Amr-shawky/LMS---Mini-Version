using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Infrastructure.Repositories;
using LMS___Mini_Version.Persistence;
using Microsoft.EntityFrameworkCore;
using MediatR;
using LMS___Mini_Version.Feature.Tracks.endpoints;
using LMS___Mini_Version.Feature.Tracks.endpoints.test;
using LMS___Mini_Version.Feature.Tracks.Query;
using LMS___Mini_Version.Feature.internFeature.endpoints;
using LMS___Mini_Version.Feature.enrollmentFeature.endpoints;
using LMS___Mini_Version.Feature.paymentFeature.endpoints;
using MassTransit;
using LMS___Mini_Version.Feature.enrollmentFeature.Consumers;
using System.Security.Cryptography;
using LMS___Mini_Version.Messages;
namespace LMS___Mini_Version
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
           
            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<EnrollmentTransferredRefundConsumer>();
                x.AddConsumer<EnrollmentTransferredNotificationConsumer>();

                x.AddEntityFrameworkOutbox<AppDbContext>(o =>
                {
                    o.UseSqlServer();
                    o.UseBusOutbox();
                });

                x.UsingRabbitMq((context, cfg) =>
                {
                    // Creates/configures the message exchange named "enrollment-refund-exchange".
                    // This replaces the default exchange name generated from the message namespace.
                    cfg.Message<EnrollmentTransferred>(m =>
                        m.SetEntityName("enrollment-refund-exchange"));

                    // Sets the exchange type to Direct instead of the default Fanout.
                    cfg.Publish<EnrollmentTransferred>(p =>
                    {
                        p.ExchangeType = "direct";
                    });

                    // Sets the routing key used when sending/publishing this message.
                    cfg.Send<EnrollmentTransferred>(s =>
                        s.UseRoutingKeyFormatter(_ => "enrollment.refund"));

                    cfg.Host("localhost", "Elevate", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    // Creates the refund queue named "enrollment-refund-queue".
                    // Configures it to receive EnrollmentTransferred messages.
                    cfg.ReceiveEndpoint("enrollment-refund-queue", e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                        e.ConfigureConsumer<EnrollmentTransferredRefundConsumer>(context);

                        // Creates a binding between the Direct Exchange and the refund queue.
                        // The queue receives messages only when the routing key matches.
                        e.Bind("enrollment-refund-exchange", s =>
                        {
                            s.RoutingKey = "enrollment.refund";
                            s.ExchangeType = "direct";
                        });
                    });

                    // Creates the notification queue named "enrollment-notification-queue".
                    // Configures it to receive EnrollmentTransferred messages.
                    cfg.ReceiveEndpoint("enrollment-notification-queue", e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                        e.ConfigureConsumer<EnrollmentTransferredNotificationConsumer>(context);

                        // Creates a binding between the Direct Exchange and the notification queue.
                        e.Bind("enrollment-refund-exchange", s =>
                        {
                            s.RoutingKey = "enrollment.refund";
                            s.ExchangeType = "direct";
                        });
                    });
                });
            });

            builder.Services.AddScoped(typeof(IGeneralRepository<>), typeof(GeneralRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddMediatR(typeof(Program).Assembly);


            var app = builder.Build();


            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AppDbContext>();
                DbInitializer.Seed(context);
            }


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // ─── Track Minimal API v2 Endpoints ────────────────────────
            app.MapGetAllTracksEndpoint();
            app.MapGetTrackByIdEndpoint();
            app.MapCreateTrackEndpoint();
            app.MapUpdateTrackV2Endpoint();
            app.MapDeleteTrackEndpoint();

            app.MapGetAllEnrollmentsEndpoint();
            app.MapGetEnrollmentByIdEndpoint();
            app.MapCreateEnrollmentEndpoint();
            app.MapCancelEnrollmentEndpoint();
            app.MapTransferEnrollmentEndpoint();

            // ─── Intern Minimal API v2 Endpoints ───────────────────────
            app.MapGetAllInternsEndpoint();
            app.MapGetInternByIdEndpoint();
            app.MapCreateInternEndpoint();
            app.MapUpdateInternEndpoint();
            app.MapDeleteInternEndpoint();

            // ─── Payment Minimal API v2 Endpoints ──────────────────────
            app.MapGetAllPaymentsEndpoint();
            app.MapGetPaymentByEnrollmentEndpoint();

            // ─── Enrollment Minimal API v2 Endpoints ───────────────────
            // Remaining Enrollment v2 endpoint (outside the assignment scope).
            app.MapGetEnrollmentsByInternEndpoint();

            app.MapGet("api/v4/test", async (IMediator mediator) => {

                var tracks = await mediator.Send(new GetAllTrackQuery());
                return tracks;
            }).WithTags("Track")
            .WithSummary("get all track minimal api");


            app.MapUpdateTrackEndpoint();
            app.Run();
        }
    }
}
