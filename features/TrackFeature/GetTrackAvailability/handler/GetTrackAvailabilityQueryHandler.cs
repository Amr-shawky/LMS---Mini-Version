using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Domain.Rules;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackAvailability.Dtos;
using LMS___Mini_Version.features.TrackFeature.GetTrackAvailability.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.TrackFeature.GetTrackAvailability.handler
{
    public class GetTrackAvailabilityQueryHandler
        : IRequestHandler<GetTrackAvailabilityQuery, RequestResponse<TrackAvailabilityDto>>
    {
        private readonly IGeneralRepository<Track> _trackRepository;
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;

        public GetTrackAvailabilityQueryHandler(
            IGeneralRepository<Track> trackRepository,
            IGeneralRepository<Enrollment> enrollmentRepository)
        {
            _trackRepository = trackRepository;
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<RequestResponse<TrackAvailabilityDto>> Handle(
            GetTrackAvailabilityQuery request, CancellationToken cancellationToken)
        {
            var track = await _trackRepository.GetAll()
                .Where(t => t.Id == request.TrackId)
                .Select(t => new { t.Name, t.IsActive, t.MaxCapacity })
                .FirstOrDefaultAsync(cancellationToken);

            if (track is null)
                return RequestResponse<TrackAvailabilityDto>.Fail(StatusCodes.Status404NotFound,
                    nameof(request.TrackId), $"Track with id {request.TrackId} was not found.");

            // Seats are reserved by Pending/Active enrollments in this track,
            // not by the number of interns whose assigned TrackId points here.
            var reservedSeats = await _enrollmentRepository.GetAll()
                .Where(e => e.TrackId == request.TrackId)
                .CountAsync(SeatReservationRules.ReservesSeat, cancellationToken);

            return RequestResponse<TrackAvailabilityDto>.OK(new TrackAvailabilityDto
            {
                TrackName = track.Name,
                IsActive = track.IsActive,
                MaxCapacity = track.MaxCapacity,
                ReservedSeats = reservedSeats,
                AvailableSeats = SeatReservationRules.CalculateAvailableSeats(track.MaxCapacity, reservedSeats)
            });
        }
    }
}
