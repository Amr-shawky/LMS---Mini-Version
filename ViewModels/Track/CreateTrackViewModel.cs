namespace LMS___Mini_Version.ViewModels.Track
{
    /// <summary>
    /// [Trap 2 Fix] Input view model for creating a Track — the client never sends
    /// the domain entity directly, preventing accidental over-posting.
    /// </summary>
    public class CreateTrackViewModel
    {
        public string Name { get; set; } = string.Empty;
        public decimal Fees { get; set; }
        public bool IsActive { get; set; }
        public int MaxCapacity { get; set; }
    }
}
