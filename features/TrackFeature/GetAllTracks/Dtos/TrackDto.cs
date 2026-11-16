namespace LMS___Mini_Version.features.TrackFeature.GetAllTracks.Dtos
{
    public class TrackDto
    {
        public int id { set; get;}

        public string Name { set; get; }

        public bool? IsActive { set; get; }
        public int MaxCapacity { get; set; }

        public int? totalenrollment { get; set; } = 0;
    }
}
