using LMS___Mini_Version.CQRS.Tracks.Queries;
using LMS___Mini_Version.Infrastructure.DTO_S.TracksDTO_s;
using MediatR;

namespace LMS___Mini_Version.CQRS.Tracks.Handlers
{
    public class GetActiveTrackQueryHandler : IRequestHandler<GetActiveTrackQuery, IEnumerable<TrackSummaryDTO>>
    {
        public Task<IEnumerable<TrackSummaryDTO>> Handle(GetActiveTrackQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
