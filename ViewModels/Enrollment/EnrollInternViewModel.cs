namespace LMS___Mini_Version.ViewModels.Enrollment
{
    /// <summary>
    /// [Trap 2 Fix] Input view model for the enrollment action.
    /// The client only needs to send InternId and TrackId.
    /// </summary>
    public class EnrollInternViewModel
    {
        public int InternId { get; set; }
        public int TrackId { get; set; }
    }
}
