namespace LMS___Mini_Version.features.InternFeatures.CreateIntern.ViewModels;

public class CreateInternVM
{

    public required string FullName { get; set; }
    public required string Email { get; set; }

    public int BirthYear { get; set; }

    public int TrackId { get; set; }
}
