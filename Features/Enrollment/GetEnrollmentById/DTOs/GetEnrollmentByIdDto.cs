using LMS___Mini_Version.Features.Intern.GetInternById.Dtos;
using LMS___Mini_Version.Features.Track.GetTrackById.Dtos;

namespace LMS___Mini_Version.Features.Enrollment.GetEnrollmentById.DTOs
{
    public class GetEnrollmentByIdDto
    {
        public int Id { get; set; }
        public int TrackId { get; set; }
        public GetTrackDto TrackDto { get; set; }
        public int InternId { get; set; }
        public GetInternDto InternDto { get; set; }
        public string PaymentStatus { get; set; }
        public string EnrollmentStatus { get; set; }
    }
}
