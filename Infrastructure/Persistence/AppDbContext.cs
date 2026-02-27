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

        public DbSet<Track> Tracks { get; set; }
        public DbSet<Intern> Interns { get; set; }
    }
}