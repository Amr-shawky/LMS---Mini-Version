using MediatR;

namespace LMS___Mini_Version.Features.Tracks.Commands
{
    public record DeleteTrackCommand(int id) : IRequest<bool>;
    
}
