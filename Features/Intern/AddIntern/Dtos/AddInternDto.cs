using System.ComponentModel.DataAnnotations;

namespace LMS___Mini_Version.Features.Intern.AddIntern.Dtos
{
    public class AddInternDto
    {
        [DataType(DataType.Text)]
        public string FullName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Range(2,4)]
        public int BirthYear { get; set; }
        [DataType(DataType.Text)]
        public string Status { get; set; }
        public int TrackId { get; set; }

    }
}
