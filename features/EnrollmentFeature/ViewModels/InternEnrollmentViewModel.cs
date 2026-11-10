namespace LMS___Mini_Version.features.EnrollmentFeature.ViewModels
{
    public class InternEnrollmentViewModel
    {
        public int EnrollmentId { get; set; }
        public int TrackId { get; set; }
        public string TrackName { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public string EnrollmentStatus { get; set; } = string.Empty;
    }
}
