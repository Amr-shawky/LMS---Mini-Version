using LMS___Mini_Version.Infrastructure.DTO_S.TracksDTO_s;
using MediatR;

namespace LMS___Mini_Version.Domain.CQRS.Tracks.Queries
{
    public record GetActiveTrackQuery : IRequest<IEnumerable<TrackSummaryDTO>>;
    
}
