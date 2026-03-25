using LMS___Mini_Version.CQRS.Tracks.Queries;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Infrastructure.DTO_S.TracksDTO_s;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Tracks.Handlers
{
    public class GetActiveTrackQueryByIdHandler(IGeneralRepository<Track> _repository) : IRequestHandler<GetActiveTrackByIdQuery, IEnumerable<TrackDto>>
    {
        public async Task<IEnumerable<TrackDto>> Handle(GetActiveTrackByIdQuery request, CancellationToken cancellationToken)
        {
            var ActiveTracks = await _repository.GetTable()
                             .Include(t => t.Interns)
                             .Include(t => t.Enrollments)
                             .Where(t => t.IsActive == true )
                             .ToListAsync();

            var ActiveTracksDTOs = ActiveTracks.Select(t => t.toTrackDto()).ToList();
            return ActiveTracksDTOs;
        }
    }
}
