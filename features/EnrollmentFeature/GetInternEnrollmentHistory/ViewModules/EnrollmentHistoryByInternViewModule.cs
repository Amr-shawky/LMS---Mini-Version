namespace LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollmentHistory.ViewModules
{
    public class EnrollmentHistoryByInternViewModule
    {
       public int EnrollmentId { get; set; }
       public int TrackId { get; set; }
       public string TrackName { get; set; } = default!;
       public DateTime EnrollmentDate { get; set; } = default!;
       public string EnrollmentStatus { get; set; } = default!;
    }
}
