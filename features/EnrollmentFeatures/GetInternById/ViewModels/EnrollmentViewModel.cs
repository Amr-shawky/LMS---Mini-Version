using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.features.EnrollmentFeatures.GetInternById.ViewModels;

public class EnrollmentViewModel
{
    public int EnrollmentId { get; set; }
    public int TrackId { get; set; }
    public string TrackName { get; set; } = default!;
    public EnrollmentStatus EnrollmentStatus { get; set; }
    public DateTime EnrollmentDate { get; set; }
}
