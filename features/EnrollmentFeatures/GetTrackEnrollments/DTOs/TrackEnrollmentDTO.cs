using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.features.EnrollmentFeatures.GetTrackEnrollments.DTOs;

public class TrackEnrollmentDTO
{
    public int EnrollmentId { get; set; }
    public string InternFullName { get; set; } = default!;
    public DateTime EnrollmentDate { get; set; }
    public EnrollmentStatus EnrollmentStatus { get; set; }
    public decimal? PaymentAmount { get; set; } // we make it nullable to return it 
    public PaymentStatus? PaymentStatus { get; set; }// we make it nullable to return it 

}
