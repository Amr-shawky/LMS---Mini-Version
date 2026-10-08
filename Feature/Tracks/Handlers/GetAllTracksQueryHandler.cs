using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Feature.Tracks.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.Tracks.Handlers
{
    public class GetAllTracksQueryHandler : IRequestHandler<GetAllTracksQuery, RequestResponse<IEnumerable<TrackDto>>>
    {
        private readonly IGeneralRepository<Track> _trackRepository;

        public GetAllTracksQueryHandler(IGeneralRepository<Track> trackRepository)
        {
            _trackRepository = trackRepository;
        }

        public async Task<RequestResponse<IEnumerable<TrackDto>>> Handle(GetAllTracksQuery request, CancellationToken cancellationToken)
        {
            var tracks = await _trackRepository.GetAll()
                .Select(t => new TrackDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Fees = t.Fees,
                    IsActive = t.IsActive,
                    MaxCapacity = t.MaxCapacity,
                    CurrentEnrollmentCount = t.Enrollments != null ? t.Enrollments.Count : 0
                }).ToListAsync(cancellationToken);

            return RequestResponse<IEnumerable<TrackDto>>.Ok(tracks);
        }
    }
}
