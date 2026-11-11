using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackAvailability.Dtos;
using LMS___Mini_Version.features.TrackFeature.GetTrackAvailability.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.TrackFeature.GetTrackAvailability.Handler
{
    public class GetTrackAvailabilityQueryHandler
        : IRequestHandler<GetTrackAvailabilityQuery,
            RequestResponse<TrackAvailabilityDto>>
    {
        private readonly IGeneralRepository<Track> repository;

        public GetTrackAvailabilityQueryHandler(
            IGeneralRepository<Track> repository)
        {
            this.repository = repository;
        }


        public async Task<RequestResponse<TrackAvailabilityDto>> Handle(
            GetTrackAvailabilityQuery request,
            CancellationToken cancellationToken)
        {
            var track = await repository.GetByIdAsync(request.TrackId);

            if (track is null)
            {
                return RequestResponse<TrackAvailabilityDto>.Fail(
                    404,
                    new Dictionary<string, string[]>
                    {
                        ["TrackId"] = new[] { "Track not found." }
                    });
            }

            var reservedSeats = await repository.GetAll()
                .Where(t => t.Id == request.TrackId)
                .SelectMany(t => t.Enrollments)
                .CountAsync(e =>
                    e.Status == EnrollmentStatus.Pending ||
                    e.Status == EnrollmentStatus.Active,
                    cancellationToken);

            var dto = new TrackAvailabilityDto
            {
                TrackName = track.Name,
                IsActive = track.IsActive,
                MaxCapacity = track.MaxCapacity,
                ReservedSeats = reservedSeats,
                AvailableSeats = Math.Max(0, track.MaxCapacity - reservedSeats)
            };

            return RequestResponse<TrackAvailabilityDto>.OK(dto);
        }

    }
}
