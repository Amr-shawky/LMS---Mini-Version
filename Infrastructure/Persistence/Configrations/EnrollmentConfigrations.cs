using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS___Mini_Version.Infrastructure.Persistence.Configrations
{
    public class EnrollmentConfigrations : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Intern)
                    .WithMany(i => i.Enrollments)
                    .HasForeignKey(e => e.InternId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Track)
                    .WithMany(t => t.Enrollments)
                    .HasForeignKey(e => e.TrackId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.Property(e => e.EnrollmentDate)
                    .HasDefaultValueSql("GETDATE()");

            builder.Property(e => e.Status)
                    .HasConversion<string>()
                    .HasDefaultValue(EnrollmentStatus.Pending);

        }
    }
}
