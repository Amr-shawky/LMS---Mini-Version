using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.Infrastructure.DTO_S.PayementsDTO_s
{
    public class PaymentDTO
    {
        public int Id { get; set; }
        public int EnrollmentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
