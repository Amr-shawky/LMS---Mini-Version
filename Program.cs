using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Infrastructure.Repositories;
using LMS___Mini_Version.Persistence;
using LMS___Mini_Version.Feature.EnrollmentSlices;
using LMS___Mini_Version.Messaging;
using LMS___Mini_Version.Feature.Tracks.endpoints;
using LMS___Mini_Version.Feature.Tracks.endpoints.test;
using LMS___Mini_Version.Feature.Tracks.Query;
using LMS___Mini_Version.Feature.internFeature.endpoints;
using LMS___Mini_Version.Feature.enrollmentFeature.endpoints;
using LMS___Mini_Version.Feature.paymentFeature.endpoints;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version;

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
        builder.Services.AddEnrollmentMessaging(builder.Configuration);

        var app = builder.Build();
        using (var scope = app.Services.CreateScope())
        {
            DbInitializer.Seed(scope.ServiceProvider.GetRequiredService<AppDbContext>());
        }

        if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.MapGetAllTracksEndpoint();
        app.MapGetTrackByIdEndpoint();
        app.MapCreateTrackEndpoint();
        app.MapUpdateTrackV2Endpoint();
        app.MapDeleteTrackEndpoint();
        app.MapGetAllInternsEndpoint();
        app.MapGetInternByIdEndpoint();
        app.MapCreateInternEndpoint();
        app.MapUpdateInternEndpoint();
        app.MapDeleteInternEndpoint();
        app.MapGetAllPaymentsEndpoint();
        app.MapGetPaymentByEnrollmentEndpoint();
        app.MapGetEnrollmentsByInternEndpoint();
        app.MapEnrollmentAssignmentEndpoints();
        app.MapGet("api/v4/test", async (IMediator mediator) => await mediator.Send(new GetAllTrackQuery()));
        app.MapUpdateTrackEndpoint();
        app.Run();
    }
}
