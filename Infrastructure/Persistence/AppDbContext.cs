using LMS___Mini_Version.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LMS___Mini_Version.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Track)
                .WithMany(t => t.Enrollments)
                .HasForeignKey(e => e.TrackId)
                .OnDelete(DeleteBehavior.NoAction);
        }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Intern> Interns { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Payment> Payments { get; set; } 
    }
}