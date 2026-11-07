namespace LMS___Mini_Version.features.TrackFeature.GetTrackById.DTOs
{
    public class GetTrackByIdDto
    {
        public int id { set; get; }

        public string name { set; get; }

        public int MaxCapacity { get; set; }
        public bool IsActive { get; set; }
    }
}
