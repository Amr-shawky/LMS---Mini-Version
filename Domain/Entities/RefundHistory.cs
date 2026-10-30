using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.Domain.Entities;

public class RefundHistory : baseEntity
{
    public int PaymentId { get; set; }
    public Payment Payment { get; set; } = null!;
    public int EnrollmentId { get; set; }
    public Enrollment Enrollment { get; set; } = null!;
    public PaymentStatus PreviousStatus { get; set; }
    public PaymentStatus NewStatus { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
