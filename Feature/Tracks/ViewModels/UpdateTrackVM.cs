namespace LMS___Mini_Version.Feature.Tracks.ViewModels
{
    public class UpdateTrackVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Fees { get; set; }
        public bool IsActive { get; set; }
        public int MaxCapacity { get; set; }
    }
}
