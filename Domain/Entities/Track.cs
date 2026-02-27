namespace LMS___Mini_Version.Domain.Entities
{
    public class Track
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public decimal Fees { get; set; }
        public bool IsActive { get; set; }

        public List<Intern> Interns { get; set; } = new List<Intern>();
    }
}