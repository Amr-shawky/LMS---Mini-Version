namespace LMS___Mini_Version.Infrastructure.DTO_S.TracksDTO_s
{
    public class TrackDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Fees { get; set; }
        public bool IsActive { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentEnrollmentCount { get; set; }
    }
}
