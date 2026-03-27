using LMS___Mini_Version.CQRS.RequestResult;
using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.CQRS.Track.Commands
{
    public record UpdateTrackCommand(int id, string name, decimal fees, bool isActive, int maxCapacity) : IRequest<RequestResult<bool>>;    
}
