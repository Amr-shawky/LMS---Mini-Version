namespace LMS___Mini_Version.ViewModels.Track
{
    /// <summary>
    /// [Trap 2 Fix] Detailed view model for single-track endpoints — includes capacity info.
    /// </summary>
    public class TrackDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Fees { get; set; }
        public bool IsActive { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentEnrollmentCount { get; set; }
    }
}
