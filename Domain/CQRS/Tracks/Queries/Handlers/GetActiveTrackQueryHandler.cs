using LMS___Mini_Version.Infrastructure.DTO_S.TracksDTO_s;
using MediatR;

namespace LMS___Mini_Version.Domain.CQRS.Tracks.Queries.Handlers
{
    public class GetActiveTrackQueryHandler : IRequestHandler<GetActiveTrackQuery, IEnumerable<TrackSummaryDTO>>
    {
        public Task<IEnumerable<TrackSummaryDTO>> Handle(GetActiveTrackQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
