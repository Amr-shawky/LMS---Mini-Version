using System.ComponentModel.DataAnnotations;

namespace LMS___Mini_Version.features.InternFeature.ViewModels
{
    public class AssignInternWithTrackViewModel
    {

        [Required(ErrorMessage ="this Field cannot be Empty.")]
        public int TrackId { get; set; }
        [EmailAddress (ErrorMessage = "Please Enter a valid Email Addresss")]
        [Required(ErrorMessage = "This Field cannot be Empty.")]
        public string Email { get; set; } = string.Empty;
        [StringLength(100,MinimumLength = 5, ErrorMessage = "FullName must be greater than 5 and than 100")]
        [Required(ErrorMessage = "This Field cannot be Empty.")]
        public string FullName { get; set; } = string.Empty;
        [Required(ErrorMessage ="This Field cannot be Empty.")]
        public int BirthYear { get; set; }
    }
}
