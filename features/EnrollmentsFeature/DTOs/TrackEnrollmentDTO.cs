using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.features.EnrollmentsFeature.DTOs
{
    public class TrackEnrollmentDTO
    {
        public int TrackId { get; set; }
        public int EnrollementId { get; set; }
        public EnrollmentStatus enrollementStatus { get; set; }
        public decimal? PaymentAmount { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public string InternFullName { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
    }
}
