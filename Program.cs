using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Messaging.Consumers;
using LMS___Mini_Version.Infrastructure.Repositories;
using LMS___Mini_Version.Persistence;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LMS___Mini_Version.Feature.Tracks.endpoints;
using LMS___Mini_Version.Feature.Tracks.endpoints.test;
using LMS___Mini_Version.Feature.Tracks.Query;
using LMS___Mini_Version.Feature.internFeature.endpoints;
using LMS___Mini_Version.Feature.enrollmentFeature.endpoints;
using LMS___Mini_Version.Feature.paymentFeature.endpoints;

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
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                       .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            builder.Services.AddScoped(typeof(IGeneralRepository<>), typeof(GeneralRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddMediatR(typeof(Program).Assembly);

            // ─── MassTransit 8 + RabbitMQ + EF Core Bus Outbox (R16, R22) ──────────────
            builder.Services.AddMassTransit(x =>
            {
                // EF Core Transactional Bus Outbox (R22):
                //   When Publish is called inside a UOF.ExecuteAsync transaction, MassTransit
                //   writes the serialized message to the OutboxMessage table in the same SQL
                //   transaction. The delivery service reads committed rows and forwards them to
                //   RabbitMQ independently. A failed DB transaction never produces a published event.
                x.AddEntityFrameworkOutbox<AppDbContext>(o =>
                {
                    o.UseSqlServer();           // SQL Server outbox tables
                    o.QueryDelay = TimeSpan.FromSeconds(10);  // poll outbox every 10 s
                    o.DuplicateDetectionWindow = TimeSpan.FromMinutes(30);
                });

                // Fanout scenario (R18): two independent consumers, two queues.
                // Each consumer gets its own receive endpoint, so each queue receives
                // a copy of every EnrollmentTransferred event (fanout behaviour).

                // Queue 1: refund-queue → RefundConsumer
                x.AddConsumer<RefundConsumer>();

                // Queue 2: notification-queue → NotificationConsumer
                x.AddConsumer<NotificationConsumer>();

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    // RabbitMQ connection settings — read from config (no secrets in Git, R16).
                    // Supply real values via user-secrets or environment variables:
                    //   dotnet user-secrets set "RabbitMQ:Username" "youruser"
                    //   dotnet user-secrets set "RabbitMQ:Password" "yourpassword"
                    cfg.Host(builder.Configuration["RabbitMQ:Host"] ?? "localhost",
                             builder.Configuration["RabbitMQ:VirtualHost"] ?? "Elevate",
                             h =>
                             {
                                 h.Username(builder.Configuration["RabbitMQ:Username"] ?? "guest");
                                 h.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
                             });

                    // Retry values from config (D6).
                    int retryCount = builder.Configuration.GetValue<int>("Messaging:Retry:RetryCount", 3);
                    int intervalSeconds = builder.Configuration.GetValue<int>("Messaging:Retry:IntervalSeconds", 5);

                    // Receive endpoint for the Refund consumer — distinct queue name (R18, D11).
                    cfg.ReceiveEndpoint("lms-enrollment-refund-queue", e =>
                    {
                        // Interval retry (R21): retries up to retryCount times with a fixed delay.
                        // Suitable for transient failures (e.g. DB connection blip).
                        // After retries are exhausted, MassTransit moves the message to the error queue.
                        e.UseMessageRetry(r =>
                            r.Interval(retryCount, TimeSpan.FromSeconds(intervalSeconds)));

                        e.ConfigureConsumer<RefundConsumer>(ctx);
                    });

                    // Receive endpoint for the Notification consumer — separate queue (R18, D11).
                    // No retry configured here; logging is idempotent and never fails.
                    cfg.ReceiveEndpoint("lms-enrollment-notification-queue", e =>
                    {
                        e.ConfigureConsumer<NotificationConsumer>(ctx);
                    });

                    cfg.ConfigureEndpoints(ctx);
                });
            });
            // ─────────────────────────────────────────────────────────────────────────────

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

            // ─── Enrollment Minimal API v2 Endpoints (R3) ──────────────
            app.MapGetAllEnrollmentsEndpoint();        // GET  /api/Enrollment/v2
            app.MapGetEnrollmentByIdEndpoint();        // GET  /api/Enrollment/v2/{id}
            app.MapEnrollEndpoint();                   // POST /api/Enrollment/v2
            app.MapCancelEnrollmentEndpoint();         // POST /api/Enrollment/v2/{id}/cancel
            app.MapTransferEnrollmentEndpoint();       // POST /api/Enrollment/v2/{id}/transfer/{newTrackId}
            app.MapGetEnrollmentsByInternEndpoint();   // GET  /api/Enrollment/v2/intern/{internId} (R4)

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
