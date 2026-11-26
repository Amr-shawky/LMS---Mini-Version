using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.features.InternFeature.ApproveAndRegisterIntern.Dtos
{
    public class InternRegistrationDto
    {
        public int EnrollmentId { get; set; }
        public int InternId { get; set; }
        public string InternStatus { get; set; } = string.Empty;
        public int TrackId { get; set; }
        public string TrackName { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public EnrollmentStatus EnrollmentStatus { get; set; }
        public int PaymentId { get; set; }
        public decimal PaymentAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
