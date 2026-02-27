namespace LMS___Mini_Version.Domain.Entities
{
    public class Intern
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int BirthYear { get; set; } 
        public string Status { get; set; } 

        public int TrackId { get; set; }
        public Track Track { get; set; }
    }
}