using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentsFeature.Queries;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.DTOs;
using LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.Handler
{
    public class CheckTracksAvaiabilityQueryHandler : IRequestHandler<CheckTrackAvaiabilityQuery, RequestResponse<CheckTrackAvaiabilityDto>>
    {
        private readonly IGeneralRepository<Track> _repository;
        private readonly IMediator _mediator;

        public CheckTracksAvaiabilityQueryHandler(IGeneralRepository<Track> repository,
            IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }
        public async Task<RequestResponse<CheckTrackAvaiabilityDto>> Handle(CheckTrackAvaiabilityQuery request, CancellationToken cancellationToken)
        {
            var track = await _repository
           .GetAll()
           .Where(t => t.Id == request.trackId)
           .Select(t => new
           {
               t.Name,
               t.IsActive,
               t.MaxCapacity,

               ReservedSeats = t.Enrollments.Count(e =>
                   e.Status == EnrollmentStatus.Pending ||
                   e.Status == EnrollmentStatus.Active)
           })
           .FirstOrDefaultAsync(cancellationToken);

            if (track is null)
            {
                IDictionary<string, string[]> errors = new Dictionary<string, string[]>()
                {
                    {"Track" , new[]{"Track Doesnot Esists"} }
                };
                return RequestResponse<CheckTrackAvaiabilityDto>.Fail(404, errors);

            }
            var availableSeats = Math.Max(0, track.MaxCapacity - track.ReservedSeats);

            var responseDto = new CheckTrackAvaiabilityDto
            {
                TrackName = track.Name,
                MaxCapacity = track.MaxCapacity,
                IsActive = track.IsActive,
                ReservedSeats = track.ReservedSeats,
                AvailableSeats = availableSeats
            };

            return RequestResponse<CheckTrackAvaiabilityDto>.OK(responseDto, 200);
        }
    }
}
