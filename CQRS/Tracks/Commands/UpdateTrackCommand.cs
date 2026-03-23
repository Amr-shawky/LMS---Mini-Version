using MediatR;

namespace LMS___Mini_Version.CQRS.Tracks.Commands
{
    public record UpdateTrackCommand(string Name, decimal Fees, bool IsActive, int MaxCapcity) : IRequest<bool>;
   
}
