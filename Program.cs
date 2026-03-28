namespace LMS___Mini_Version
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ─── Framework Services ───────────────────────────────────────
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ─── Database ─────────────────────────────────────────────────
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ─── Repository & Unit of Work ────────────────────────────────
            // [Trap 1 + 6 Fix] Controllers never touch DbContext.
            // All data access goes through IUnitOfWork → IGeneralRepository<T>.
            builder.Services.AddScoped(typeof(IGeneralRepository<>), typeof(GeneralRepository<>));
            builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
         
            // ───validators────────────────────────────────────────────────
            builder.Services.AddScoped<CreateInternValidators>();
            builder.Services.AddScoped<UpdateInternValidator>();
            builder.Services.AddScoped<EnrollmentValidator>();
            // ─── Mediators  ──────────────────────────
            builder.Services.AddMediatR(cfg=>cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            
            var app = builder.Build();

            // ─── Seed Data ────────────────────────────────────────────────
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AppDbContext>();
                DbInitializer.Seed(context);
            }

            // ─── HTTP Pipeline ────────────────────────────────────────────
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
