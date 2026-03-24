using LMS___Mini_Version.CQRS.Tracks.Queries;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Infrastructure.DTO_S.TracksDTO_s;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Tracks.Handlers
{
    public class GetActiveTrackQueryHandler(IGeneralRepository<Track> _repository) : IRequestHandler<GetActiveTrackQuery, IEnumerable<TrackDto>>
    {
        public async Task<IEnumerable<TrackDto>> Handle(GetActiveTrackQuery request, CancellationToken cancellationToken)
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
