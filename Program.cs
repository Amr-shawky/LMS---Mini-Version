using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Infrastructure.Repositories;
using LMS___Mini_Version.Persistence;
using Microsoft.EntityFrameworkCore;
using MediatR;
using LMS___Mini_Version.Feature.Tracks.endpoints;
using LMS___Mini_Version.Feature.Tracks.Query;
using LMS___Mini_Version.Feature.internFeature.endpoints;
using LMS___Mini_Version.Feature.enrollmentFeature.endpoints;
using LMS___Mini_Version.Feature.paymentFeature.endpoints;
using LMS___Mini_Version.Feature.enrollmentFeature.Consumers;
using MassTransit;
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
                x.AddConsumer<TransferRefundConsumer>();
                x.AddConsumer<TransferNotificationConsumer>();
                x.AddEntityFrameworkOutbox<AppDbContext>(o =>
                {
                    o.UseSqlServer();
                    o.UseBusOutbox();
                });

                x.UsingRabbitMq((context, cfg) =>
                {
                    var host = builder.Configuration["RabbitMQ:localhost"];
                    var user = builder.Configuration["RabbitMQ:Username"] ?? "guest";
                    var password = builder.Configuration["RabbitMQ:Password"] ?? "guest";
                    cfg.Host(host, "Elevate", h =>
                    {
                        h.Username(user);
                        h.Password(password);
                    });

                    cfg.ReceiveEndpoint("enrollment-transfer-refunds", e =>
                    {
                        e.ConfigureConsumer<TransferRefundConsumer>(context);
                        e.UseMessageRetry(r => r.Immediate(3));
                    });
                    cfg.ReceiveEndpoint("enrollment-transfer-notifications", e =>
                    {
                        e.ConfigureConsumer<TransferNotificationConsumer>(context);
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(2)));
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
            app.MapEnrollmentV2Endpoints();

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
