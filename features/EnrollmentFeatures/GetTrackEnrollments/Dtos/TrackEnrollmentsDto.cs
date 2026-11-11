using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.features.EnrollmentFeatures.GetTrackEnrollments.Dtos
{
    public class TrackEnrollmentsDto
    {
        public int EnrollmentId { get; set; }
        public string InternFullName { get; set; } = string.Empty;
        public decimal? PaymentAmount { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public EnrollmentStatus EnrollmentStatus { get; set; }
    }
}
