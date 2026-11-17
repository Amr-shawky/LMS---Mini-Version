using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.Features.Enrollments.VMs
{
    public class ReviewTrackEnrollmentViewModel
    {
        public int EnrollmentId { get; set; }
        public string InternFullName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public EnrollmentStatus enrollmentStatus { get; set; }
        public decimal PaymentAmount { get; set; }
        public string paymentStatus { get; set; }
    }
}
