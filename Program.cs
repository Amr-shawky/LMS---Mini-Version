using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Infrastructure.Repositories;
using LMS___Mini_Version.Mediators;
using LMS___Mini_Version.Persistence;
using LMS___Mini_Version.Services.Implementations;
using LMS___Mini_Version.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using LMS___Mini_Version.Feature.Tracks.endpoints;
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
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

      
            builder.Services.AddScoped(typeof(IGeneralRepository<>), typeof(GeneralRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<ITrackService, TrackService>();
            builder.Services.AddScoped<IInternService, InternService>();
            builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();


            builder.Services.AddScoped<EnrollInternMediator>();
            builder.Services.AddScoped<CancelEnrollmentMediator>();
            builder.Services.AddScoped<TransferEnrollmentMediator>();


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
            app.MapGetAlltracksEndpoint();


            app.Run();
        }
    }
}
