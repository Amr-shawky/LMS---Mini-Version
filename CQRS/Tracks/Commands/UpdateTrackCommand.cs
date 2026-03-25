using LMS___Mini_Version.Infrastructure.DTO_S.TracksDTO_s;
using MediatR;

namespace LMS___Mini_Version.CQRS.Tracks.Commands
{
    public record UpdateTrackCommand(int trackId,string Name, decimal Fees, bool IsActive, int MaxCapcity) : IRequest<RequestResult<TrackDto>>;
   
}
