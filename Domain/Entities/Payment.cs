using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        public int EnrollmentId { get; set; }

        //Navigation: one payment belongs to one enrollment
        public Enrollment Enrollment { get; set; } = null!;

        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
