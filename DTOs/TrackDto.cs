namespace LMS___Mini_Version.DTOs
{
    /// <summary>
    /// [Trap 2 Fix] Data Transfer Object for Track — used between Service ↔ Mediator layers.
    /// Decouples the internal entity structure from what flows between architectural layers.
    /// </summary>
    public class TrackDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Fees { get; set; }
        public bool IsActive { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentEnrollmentCount { get; set; }
    }
}
