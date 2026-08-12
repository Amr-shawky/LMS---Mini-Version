using MediatR;

namespace LMS___Mini_Version.Features.Tracks.Commands
{
    public record UpdateTrackCommand(int Id, string Name,
                            decimal Fees, bool IsActive, int MaxCapacity) : IRequest;
    
    
}
