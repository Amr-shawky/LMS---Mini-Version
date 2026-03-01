namespace LMS___Mini_Version.ViewModels.Track
{
    /// <summary>
    /// [Trap 2 Fix] Lightweight view model for list endpoints — only exposes
    /// what the client needs, preventing over-fetching (no SELECT *).
    /// </summary>
    public class TrackSummaryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Fees { get; set; }
        public bool IsActive { get; set; }
    }
}
