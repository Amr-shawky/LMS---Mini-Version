using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.Domain.Entities
{
    public class RefundHistory:baseEntity
    {
        public DateTime CreateAt { get; set; }

        public int EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; } = null!;

        public int PaymentId { get; set; }
        public Payment Payment { get; set; } = null!;
    }
}
