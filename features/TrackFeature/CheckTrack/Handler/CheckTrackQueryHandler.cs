using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeatures.GetEnrollments.Dtos;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.CheckTrack.Dtos;
using LMS___Mini_Version.features.TrackFeature.CheckTrack.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.TrackFeature.CheckTrack.Handler
{
    public class CheckTrackQueryHandler(IGeneralRepository<Track> trackRepo) : IRequestHandler<CheckTrackQuery, RequestResponse<CheckTrackDto>>
    {
        private readonly IGeneralRepository<Track> trackRepo = trackRepo;

        public async Task<RequestResponse<CheckTrackDto>> Handle(CheckTrackQuery request, CancellationToken cancellationToken)
        {
         var track = await trackRepo.GetAll()
                .Where(x => x.Id == request.TrackId)
                .Select(x => new CheckTrackDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive,
                    MaxCapacity = x.MaxCapacity,
                    ReservedSeats = x.Enrollments.Count(e =>
                    e.Status == EnrollmentStatus.Pending ||
                    e.Status == EnrollmentStatus.Active)
                }).FirstOrDefaultAsync();

            if(track == null)
            {
                return RequestResponse<CheckTrackDto>.Fail(404, new Dictionary<string, string[]>
                {
                    [nameof(request.TrackId)] = new[] { "Track not found" }
                });
            }

            track.AvailableSeats = Math.Max(0,track.MaxCapacity - track.ReservedSeats);
            return RequestResponse<CheckTrackDto>.OK(track);
        }
        
    }
}
