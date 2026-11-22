namespace LMS___Mini_Version.features.EnrollmentFeature.ViewModels
{
    public class EnrollmentsForTrackViewModel
    {
        public int EnrollmentId { get; set; }
        public string InternFullName { get; set; } = string.Empty;
        public DateOnly EnrollmentDate { get; set; }
        public string EnrollmentStatus { get; set; } = string.Empty;
        public decimal? PaymentAmount { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
    }
}
