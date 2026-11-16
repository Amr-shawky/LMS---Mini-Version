using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.features.InternFeatures.GetallIntern.ViewModels;

public class EnrollmentVM
{
   public int EnrollmentId { get; set; }
   public int TrackId { get; set; }
    public string TrackName { get; set; } = default!;
   public EnrollmentStatus EnrollmentStatus { get; set; }
   public DateTime EnrollmentDate { get; set; }
 
}
