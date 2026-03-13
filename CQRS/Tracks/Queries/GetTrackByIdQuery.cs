using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Tracks.Queries;

public record GetTrackByIdQuery (int id) : IQuery<TrackDto?>;

public class GetTrackByIdQueryHandler (IGeneralRepository<Track> _trackRepo)
    : IRequestHandler<GetTrackByIdQuery, TrackDto?>
{ 
    public async Task<TrackDto?> Handle(GetTrackByIdQuery request, CancellationToken cancellationToken)
    {

        /// later we'll refactor validation 

        var track = await _trackRepo.GetTable()
            .Where(t => t.Id == request.id)
            .Select(t => new TrackDto
            {
                Id = request.id,
                Name = t.Name,
                Fees = t.Fees,
                IsActive = t.IsActive,
                MaxCapacity = t.MaxCapacity,
                CurrentEnrollmentCount = t.Enrollments.Count()
            }).FirstOrDefaultAsync(cancellationToken);

        if (track == null) 
        {
            return null;
        }
        return track;

    }
}
