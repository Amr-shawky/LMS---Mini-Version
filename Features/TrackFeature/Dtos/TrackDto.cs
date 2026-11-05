namespace LMS___Mini_Version.Features.TrackFeature.Dtos
{
    public class TrackDto
    {
        public int id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Fees { get; set; }
        public int MaxCapacity { get; set; }
    }
}
