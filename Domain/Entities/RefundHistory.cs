using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.Domain.Entities
{
    public class RefundHistory : baseEntity
    {
        public int PaymentId { get; set; }
        public int EnrollmentId { get; set; }
        public PaymentStatus PreviousStatus { get; set; }
        public PaymentStatus NewStatus { get; set; }
    }
}
