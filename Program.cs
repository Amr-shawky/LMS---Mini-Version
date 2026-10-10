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
using LMS___Mini_Version.contracts;
using LMS___Mini_Version.Feature.consumers;
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

                x.AddConsumer<trackcreatedConsumer>();
                x.AddConsumer<NotificationConsumer>();

                //new 
                x.AddConsumer<VisaPaymentConsumer>();
                x.AddConsumer<CashPaymentConsumer>();



                x.UsingRabbitMq((context, cfg) => {

                    cfg.Host("localhost", "/", h => {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.UseInMemoryOutbox(context);

                    cfg.Message<trackcreatedmessage>(m =>
                    {
                        m.SetEntityName("track-exchange");
                    });



                    cfg.Message<PaymentProcessedMessage>(p =>
                    {

                        p.SetEntityName("payment-exchange");

                    });

                    cfg.Publish<PaymentProcessedMessage>(p =>
                    {

                        p.ExchangeType = "direct";

                    });

                    cfg.ReceiveEndpoint("track-created-queue",e => {

                        e.ConfigureConsumer<trackcreatedConsumer>(context);
                    });

                    cfg.ReceiveEndpoint("notification-queue", e =>
                    {
                        e.ConfigureConsumer<NotificationConsumer>(context);
                    });


                    cfg.ReceiveEndpoint("cash-queue", c => {

                        c.ConfigureConsumeTopology = false;

                        c.Bind("payment-exchange", p => {

                            p.ExchangeType = "direct";
                            p.RoutingKey = "cash";
                        });

                        c.ConfigureConsumer<CashPaymentConsumer>(context);
                    });

                    cfg.ReceiveEndpoint("visa-queue", v => {

                        v.ConfigureConsumeTopology = false;

                        v.Bind("payment-exchange", e => {

                            e.ExchangeType = "direct";
                            e.RoutingKey = "visa";
                        
                        });
                        //v.UseInMemoryOutbox(context);
                        v.UseMessageRetry(r => {

                            r.Interval(3,TimeSpan.FromSeconds(2));
                        });
                        
                        //v.UseDelayedRedelivery(r =>
                        //{
                        //    r.Intervals(
                        //        TimeSpan.FromMinutes(1),
                        //        TimeSpan.FromMinutes(5));
                        //});

                        v.ConfigureConsumer<VisaPaymentConsumer>(context);
                    });
                });
            });












            //builder.Services.AddMassTransit(x => 
            //{
            //    x.AddConsumer<trackcreatedConsumer>();

            //    x.UsingRabbitMq((context,cfg) => {

            //        cfg.Host("localhost", "/", s => {

            //            s.Username("guest");
            //            s.Password("guest");

            //        });
            //    cfg.ReceiveEndpoint("track-created-queue", e =>{


            //        e.ConfigureConsumer<trackcreatedConsumer>(context);

            //    });

            //  cfg.ConfigureEndpoints(context);
            //    });
            //});

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
            app.MapGetAllEnrollmentsEndpoint();
            app.MapGetEnrollmentByIdEndpoint();
            app.MapGetEnrollmentsByInternEndpoint();
            app.MapEnrollInternEndpoint();
            app.MapCancelEnrollmentEndpoint();
            app.MapTransferEnrollmentEndpoint();

            app.MapPost("api/test/visa", async(IPublishEndpoint publish) => {

                await publish.Publish(new PaymentProcessedMessage {PaymentId =2,Amount=40}
                
                ,
                context => {

                    context.SetRoutingKey("visa");
                });

                return Results.Ok("visa payment event published");
            
            });

            app.MapPost("api/test/cash", async (IPublishEndpoint publish) => {

                await publish.Publish(new PaymentProcessedMessage { PaymentId = 2, Amount = 40 }
                ,
                context => {

                    context.SetRoutingKey("cash");
                });

                return Results.Ok("cash payment event published");

            });

            app.MapGet("api/v4/test", async (IMediator mediator) => {

                var tracks = await mediator.Send(new GetAllTrackQuery());
                return tracks;
            }).WithTags("Track")
            .WithSummary("get all track minimal api");


            app.MapUpdateTrackEndpoint();
            try
            {
                app.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("========== FATAL ERROR ==========");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
