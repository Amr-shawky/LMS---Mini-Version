using LMS___Mini_Version.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS___Mini_Version.Infrastructure.Persistence.Configrations
{
    public class TrackConfigrations : IEntityTypeConfiguration<Track>
    {
        public void Configure(EntityTypeBuilder<Track> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.Fees)
                .HasColumnType("decimal(18,2)");


            builder.HasMany(t=>t.Interns)
                    .WithOne(i=>i.Track)
                    .HasForeignKey(i=>i.TrackId)
                    .OnDelete(DeleteBehavior.Restrict);


                builder.HasMany(t=>t.Enrollments)
                    .WithOne(e=>e.Track)
                    .HasForeignKey(e=>e.TrackId)
                    .OnDelete(DeleteBehavior.Restrict);



        }
    }
}
