using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.features.InternFeature.GetInternEnrollmentHistory.ViewModels
{
    public class GetInternEnrollmentHistoryOutputViewModel
    {
        public int EnrollmentId { get; set; }
        public int TrackId { get; set; }
        public string TrackName { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public EnrollmentStatus EnrollmentStatus { get; set; }
    }
}
