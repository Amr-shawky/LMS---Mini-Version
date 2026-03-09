using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.CQRS.Tracks.Commands
{
    public record CreateTrackCommand(string name, decimal fees, bool isActive, int maxCapacity) : IRequest<TrackDto>;
   

}
