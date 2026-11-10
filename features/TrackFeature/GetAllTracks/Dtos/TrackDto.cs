namespace LMS___Mini_Version.features.TrackFeature.GetAllTracks.Dtos
{
    public class TrackDto
    {
        public int Id { set; get; }

        public required string Name { set; get; }

        public int MaxCapacity { get; set; }

        public int? Totalenrollment { get; set; } = 0;
    }
}
