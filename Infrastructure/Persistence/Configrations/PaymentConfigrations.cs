using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS___Mini_Version.Infrastructure.Persistence.Configrations
{
    public class PaymentConfigrations : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
           builder.HasKey(p => p.Id);
            builder.Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.PaymentDate)
                   .HasDefaultValueSql("GETDATE()");

                builder.Property(p => p.Method)
                    .HasConversion<string>()
                    .HasDefaultValue(PaymentMethod.CreditCard);


            builder.HasOne(p => p.Enrollment)
                .WithOne(e => e.Payment)
                .HasForeignKey<Payment>(p => p.EnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
