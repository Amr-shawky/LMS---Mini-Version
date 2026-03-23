using LMS___Mini_Version.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS___Mini_Version.Infrastructure.Persistence.Configrations
{
    public class InternConfigrations : IEntityTypeConfiguration<Intern>
    {
        public void Configure(EntityTypeBuilder<Intern> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.FullName)
                .IsRequired()
                .HasMaxLength(100);


           builder.HasOne(i => i.Track)
                .WithMany(t => t.Interns)
                .HasForeignKey(i => i.TrackId)
                .OnDelete(DeleteBehavior.Restrict);


             builder.HasMany(i => i.Enrollments)
                .WithOne(e => e.Intern)
                .HasForeignKey(e => e.InternId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
