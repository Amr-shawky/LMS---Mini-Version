using LMS___Mini_Version.Domain.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Track> Tracks { get; set; }
        public DbSet<Intern> Interns { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<RefundHistory> RefundHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();

            // Enrollment → Intern (many-to-one)
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Intern)
                .WithMany(i => i.Enrollments)
                .HasForeignKey(e => e.InternId)
                .OnDelete(DeleteBehavior.Restrict);

            // Enrollment → Track (many-to-one)
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Track)
                .WithMany(t => t.Enrollments)
                .HasForeignKey(e => e.TrackId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment → Enrollment (one-to-one)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Enrollment)
                .WithOne(e => e.Payment)
                .HasForeignKey<Payment>(p => p.EnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RefundHistory>()
                .HasOne(h => h.Payment)
                .WithMany(p => p.RefundHistories)
                .HasForeignKey(h => h.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RefundHistory>()
                .HasOne(h => h.Enrollment)
                .WithMany()
                .HasForeignKey(h => h.EnrollmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Store enums as strings for readability in the DB
            modelBuilder.Entity<Enrollment>()
                .Property(e => e.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Payment>()
                .Property(p => p.Method)
                .HasConversion<string>();

            modelBuilder.Entity<Payment>()
                .Property(p => p.Status)
                .HasConversion<string>();

            modelBuilder.Entity<RefundHistory>()
                .Property(h => h.PreviousStatus)
                .HasConversion<string>();

            modelBuilder.Entity<RefundHistory>()
                .Property(h => h.NewStatus)
                .HasConversion<string>();

            // Decimal precision for fees and amounts
            modelBuilder.Entity<Track>()
                .Property(t => t.Fees)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);
        }
    }
}