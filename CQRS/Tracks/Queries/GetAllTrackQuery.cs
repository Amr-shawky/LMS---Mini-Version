using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Tracks.Queries;

public record GetAllTrackQuery : IQuery<IEnumerable<TrackDto>>;

public class GetAllTrackQueryHandler (IGeneralRepository<Track> _trackRepo)
    : IRequestHandler<GetAllTrackQuery, IEnumerable<TrackDto>>
{   
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