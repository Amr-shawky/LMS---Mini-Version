namespace LMS___Mini_Version.features.TrackFeature.GetTrackById.DTOs
{
    public class GetTrackByIdDto
    {
        public int Id { set; get; }

        public string Name { set; get; } = String.Empty;

        public int MaxCapacity { get; set; }
        public bool IsActive { get; set; }
        public decimal Fees { get; set; }

    }
}
