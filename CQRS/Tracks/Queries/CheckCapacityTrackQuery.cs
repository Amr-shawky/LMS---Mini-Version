using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Tracks.Queries;

public record CheckCapacityTrackQuery(int trackId) : IQuery<bool>;

public class CheckCapacityTrackQueryHandler (IGeneralRepository<Track> _trackRepo)
    : IRequestHandler<CheckCapacityTrackQuery, bool>
{
    public async Task<bool> Handle(CheckCapacityTrackQuery request, CancellationToken cancellationToken)
    {        

        var track = await _trackRepo.GetTable().AsNoTracking()
            .Where(t => t.Id == request.trackId).Select(t=> new TrackDto 
            {
                Id = t.Id,
                Name = t.Name,
                CurrentEnrollmentCount = t.Enrollments.Count(c=> c.Status != Domain.Enums.EnrollmentStatus.Cancelled),
                MaxCapacity = t.MaxCapacity,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (track == null) return false;

        return track.CurrentEnrollmentCount < track.MaxCapacity;
    }
}
