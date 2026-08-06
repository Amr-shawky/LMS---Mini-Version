using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Tracks.Queries;

public record CheckTrackCapacityQuery (int trackId): IRequest<bool>;


public class CheckTrackCapacityQueryHandler (IGeneralRepository<Track> _trackRepo)
    : IRequestHandler<CheckTrackCapacityQuery, bool>
{
    public async Task<bool> Handle(CheckTrackCapacityQuery request, CancellationToken cancellationToken)
    {           

        var track = await _trackRepo.GetTable()
            .Where(t => t.Id == request.trackId)
            .Select(t=> new TrackDto 
            {
                Id = t.Id,
                Name = t.Name,
                Fees = t.Fees,
                IsActive = t.IsActive,
                MaxCapacity = t.MaxCapacity,
                CurrentEnrollmentCount = t.Enrollments.Count(e => e.Status != Domain.Enums.EnrollmentStatus.Cancelled) // get all valid enrollments
            })
            .FirstOrDefaultAsync(cancellationToken);     

        return track.CurrentEnrollmentCount < track.MaxCapacity;
    }
}
