using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Feature.Tracks.Query;
using MediatR;

namespace LMS___Mini_Version.Feature.Tracks.Handlers
{
    public class GetByIdTrackQueryHandler : IRequestHandler<GetByIdTrackQuery, RequestResponse<TrackDto>>
    {
        private readonly IGeneralRepository<Track> _trackRepository;
        public GetByIdTrackQueryHandler(IGeneralRepository<Track> trackRepository)
        {
            _trackRepository = trackRepository;
        }

        public async Task<RequestResponse<TrackDto>> Handle(GetByIdTrackQuery request, CancellationToken cancellationToken)
        {
            var track = await _trackRepository.GetByIdAsync(request.Id);

            if (track == null)
                return RequestResponse<TrackDto>.Fail($"Track with ID {request.Id} not found", 404);

            var dto = new TrackDto
            {
                Id = track.Id,
                Name = track.Name,
                Fees = track.Fees,
                IsActive = track.IsActive,
                MaxCapacity = track.MaxCapacity,
                CurrentEnrollmentCount = track.Enrollments?.Count ?? 0
            };

            return RequestResponse<TrackDto>.Ok(dto);
        }
    }
}
