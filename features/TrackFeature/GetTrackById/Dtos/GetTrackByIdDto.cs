namespace LMS___Mini_Version.features.TrackFeature.GetTrackById.Dtos
{
    public class GetTrackByIdDto
    {
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int MaxCapacity { get; set; }
        public decimal Fees { get; set; }
    }
}
