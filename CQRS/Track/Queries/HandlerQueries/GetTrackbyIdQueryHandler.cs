using LMS___Mini_Version.CQRS.RequestResult;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Track.Queries.HandlerQueries
{
    public class GetTrackbyIdQueryHandler : IRequestHandler<GetTrackbyIdQuery, RequestResult<TrackDto>>
    {
        private readonly IUnitOfWork _uow;
        public async Task<RequestResult<TrackDto>> Handle(GetTrackbyIdQuery request, CancellationToken cancellationToken)
        {
            var trackdto =  await _uow.Tracks.GetTable().Where(track=>track.Id==request.id).Select(track=>track.ToDto()).FirstOrDefaultAsync();
            
            return (trackdto == null)
                ?RequestResult<TrackDto>.Failure(ErrorCode.NotFound)
                :RequestResult<TrackDto>.Success(trackdto);
        }
    }
}
