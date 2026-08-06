using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Tracks.Queries
{
    public record GetAllTrackQuery : IRequest<IEnumerable<TrackDto>>;

    public class GetAllTrackQueryHandler : IRequestHandler<GetAllTrackQuery, IEnumerable<TrackDto>>
    {
        IGeneralRepository<Domain.Entities.Track> _trackRepo;
        public GetAllTrackQueryHandler(IGeneralRepository<Domain.Entities.Track> tracjRepo)
        {
            _trackRepo = tracjRepo;
        }
        public async Task<IEnumerable<TrackDto>> Handle(GetAllTrackQuery request, CancellationToken cancellationToken)
        {
            var tracks = await _trackRepo.GetTable()
                .Select(t => new TrackDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Fees = t.Fees,
                    IsActive = t.IsActive,              
                }).ToListAsync();

            return tracks;
        }
    }
}