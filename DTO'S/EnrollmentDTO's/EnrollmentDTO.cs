using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.Infrastructure.DTO_S.EnrollmentDTO_s
{
    public class EnrollmentDTO
    {
        public int Id { get; set; }
        public int InternId { get; set; }
        public string InternName { get; set; } = string.Empty;
        public int TrackId { get; set; }
        public string TrackName { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public EnrollmentStatus Status { get; set; }
    }
}
