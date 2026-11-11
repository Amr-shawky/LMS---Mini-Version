using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.features.TrackFeature.GetTrackEnrollments.Dtos
{
    public class GetTrackEnrollmentsDto
    {
        public string InternFullName { get; set; } = string.Empty;
        public int EnrollmentId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public EnrollmentStatus EnrollmentStatus { get; set; }

        public decimal? PaymentAmount { get; set; }

        public PaymentStatus? PaymentStatus { get; set; }
    }
}
