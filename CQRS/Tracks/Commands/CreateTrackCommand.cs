using LMS___Mini_Version.Domain.Entities;
using MediatR;
using System.Diagnostics;

namespace LMS___Mini_Version.CQRS.Tracks.Commands
{
    public record CreateTrackCommand(string Name,decimal Fees,bool IsActive,int MaxCapcity) : IRequest<Track>;
   
}
