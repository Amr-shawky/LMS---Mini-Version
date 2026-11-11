using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.features.EnrollmentFeature.GetTrackEnrollments
{
    public class TrackEnrollmentsDto
    {
        public int EnrollmentId { get; set; }
        public string InternFullName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public EnrollmentStatus EnrollmentStatus { get; set; }
        public decimal PaymentAmount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}
