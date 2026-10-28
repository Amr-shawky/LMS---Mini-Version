using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.Domain.Entities
{

    public class Payment : baseEntity
    {

        public int EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; } = null!;

        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
