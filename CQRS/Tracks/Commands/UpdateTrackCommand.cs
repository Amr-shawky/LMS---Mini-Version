using MediatR;

namespace LMS___Mini_Version.CQRS.Tracks.Commands
{
    public record UpdateTrackCommand(int id, string name, decimal fees, bool isActive, int maxCapacity)
        : IRequest<bool>;
 
    
}
