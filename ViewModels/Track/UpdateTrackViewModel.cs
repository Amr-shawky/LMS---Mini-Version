namespace LMS___Mini_Version.ViewModels.Track
{
    /// <summary>
    /// [Trap 2 Fix] Input view model for updating a Track.
    /// </summary>
    public class UpdateTrackViewModel
    {
        public string Name { get; set; } = string.Empty;
        public decimal Fees { get; set; }
        public bool IsActive { get; set; }
        public int MaxCapacity { get; set; }
    }
}
