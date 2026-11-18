using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.features.EnrollmentFeature.DTOs
{
    public class EnrollmentDto
    {
        public EnrollmentStatus EnrollmentStatus { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
    }
}
