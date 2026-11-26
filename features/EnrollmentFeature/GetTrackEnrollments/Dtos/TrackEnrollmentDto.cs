using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.features.EnrollmentFeature.GetTrackEnrollments.Dtos
{
    public class TrackEnrollmentDto
    {
        public int EnrollmentId { get; set; }
        public string InternFullName { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public EnrollmentStatus EnrollmentStatus { get; set; }

        // Null when no payment has been recorded for the enrollment.
        public decimal? PaymentAmount { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
    }
}
