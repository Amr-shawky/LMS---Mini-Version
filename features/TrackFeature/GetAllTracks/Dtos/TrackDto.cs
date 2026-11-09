namespace LMS___Mini_Version.features.TrackFeature.GetAllTracks.Dtos
{
    public class TrackDto
    {
        public int Id { set; get;}

        public string Name { set; get; } = string.Empty;

        public int MaxCapacity { get; set; }

        public int? Totalenrollment { get; set; } = 0;
    }
}
