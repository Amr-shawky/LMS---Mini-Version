namespace LMS___Mini_Version
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddRateLimiter(config =>
            {
                config.AddFixedWindowLimiter("fixed", s =>
                {
                    s.PermitLimit = 10;
                    s.Window = TimeSpan.FromSeconds(10);
                });
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddMediatR(typeof(Program).Assembly);

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<EnrollmentTransferredConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.Message<EnrollmentTransferedMessage>(m =>
                    {
                        m.SetEntityName("enrollment-transferred-exchange");
                    });
                    cfg.ReceiveEndpoint("enrollment-transferred-queue", e =>
                    {
                        e.UseMessageRetry(retry =>
                            retry.Exponential(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(5)));
                        e.ConfigureConsumer<EnrollmentTransferredConsumer>(context);
                    });
                });
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AppDbContext>();
                DbInitializer.Seed(context);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRateLimiter();

            app.UseAuthorization();

            /*minimal apis*/
            app.AddMinimalApi();

            app.MapControllers();

            app.Run();
        }
    }
}
