using MediatR;

namespace LMS___Mini_Version.Feature.Tracks.Commands
{
    public class CreateTrackCommand : IRequest
    {
        //string Name, decimal Fees, bool IsActive, int MaxCapacity
         public string Name { get; set; } = string.Empty;
         public decimal Fees { get; set; }
         public bool IsActive { get; set; }
         public int MaxCapacity { get; set; }
        public CreateTrackCommand(string name, decimal fees, bool isActive, int maxCapacity)
        {
            this.Name = name;
            this.Fees = fees;
            this.IsActive = isActive;
            this.MaxCapacity = maxCapacity;
        }
    }

}
