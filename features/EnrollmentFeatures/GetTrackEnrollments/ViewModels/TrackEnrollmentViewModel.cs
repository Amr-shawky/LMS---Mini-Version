using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.features.EnrollmentFeatures.GetTrackEnrollments.ViewModels;

public class TrackEnrollmentViewModel
{
    public int EnrollmentId { get; set; }
    public string InternFullName { get; set; } = default!;
    public DateTime EnrollmentDate { get; set; }
    public EnrollmentStatus EnrollmentStatus { get; set; }
    public decimal? PaymentAmount { get; set; }
    public PaymentStatus? PaymentStatus { get; set; }
}
