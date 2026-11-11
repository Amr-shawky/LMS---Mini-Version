using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.features.EnrollmentFeature.GetEnrollmentsByTrack.Dtos
{
    public class EnrollmentByTrackDto
    {
        public int EnrollmentId { get; set; }
        public string InternName { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public EnrollmentStatus EnrollmentStatus { get; set; }
        public decimal? PaymentAmount { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
    }
}
