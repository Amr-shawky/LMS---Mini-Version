using LMS___Mini_Version.CQRS.RequestResult;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Infrastructure.Repositories;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Track.Queries.HandlerQueries
{
    public class GetAllTrackQueryHandler : IRequestHandler<GetAllTrackQuery, RequestResult<IEnumerable<TrackDto>>>
    {
        private readonly UnitOfWork _uow;
        public async Task<RequestResult<IEnumerable<TrackDto>>> Handle(GetAllTrackQuery request, CancellationToken cancellationToken)
        {
            var tracksDto = await _uow.Tracks.GetTable().Select(track => track.ToDto()).ToListAsync();
            return (tracksDto==null)
                ?RequestResult<IEnumerable<TrackDto>>.Failure(ErrorCode.NotFound) 
                :RequestResult<IEnumerable<TrackDto>>.Success(tracksDto);
        }
    }
}
