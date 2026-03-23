using LMS___Mini_Version.CQRS.Tracks.Queries;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Infrastructure.DTO_S.TracksDTO_s;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Tracks.Handlers
{
    public class GetAllTracksQueyHandler(IGeneralRepository<Track>  _repository) : IRequestHandler<GetAllTracksQuery, IEnumerable<TrackDto>>
    {
        public async Task<IEnumerable<TrackDto>> Handle(GetAllTracksQuery request, CancellationToken cancellationToken)
        {
            var tracks =await _repository.GetTable()
                        .Include(t => t.Interns)
                        .Include(t => t.Enrollments)
                        .ToListAsync()
                        .ConfigureAwait(false);


            var trackDtos =tracks.Select(t=>t.toTrackDto()).ToList();
            return trackDtos;

        }
    }
}
