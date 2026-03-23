using LMS___Mini_Version.Infrastructure.DTO_S.TracksDTO_s;
using MediatR;

namespace LMS___Mini_Version.CQRS.Tracks.Queries
{
    public record GetAllTracksQuery:IRequest<IEnumerable<TrackDto>>;
    
}
