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
