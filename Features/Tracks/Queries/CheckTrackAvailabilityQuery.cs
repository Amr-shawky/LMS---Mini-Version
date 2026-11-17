using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Shared;
using LMS___Mini_Version.Features.Tracks.DTOs;
using MediatR;

namespace LMS___Mini_Version.Features.Tracks.Queries
{
    public record CheckTrackAvailabilityQuery(int trackId) : IRequest<RequestResponse<CheckTrackAvailabilityDTO>>;

    public class CheckTrackAvailabilityQueryHandler:IRequestHandler<CheckTrackAvailabilityQuery, RequestResponse<CheckTrackAvailabilityDTO>>
    {
        private readonly IGeneralRepository<Track> _trackRepo;

        public CheckTrackAvailabilityQueryHandler(IGeneralRepository<Track> trackRepo)
        {
            _trackRepo = trackRepo;
        }

        public async Task<RequestResponse<CheckTrackAvailabilityDTO>> Handle(CheckTrackAvailabilityQuery query,CancellationToken cancellationToken)
        {
            var track = _trackRepo.GetById(query.trackId);

            if (track == null)
                return RequestResponse<CheckTrackAvailabilityDTO>.Fail();

            var result = new CheckTrackAvailabilityDTO
            {
                TrackName = track.Name,
                IsActive = track.IsActive,
                MaxCapacity = track.MaxCapacity,
                ReservedSeats = track.Enrollments.Count,
                AvailableSeats = track.MaxCapacity - track.Enrollments.Count
            };
            return RequestResponse<CheckTrackAvailabilityDTO>.OK(result);
        }
    }
}
