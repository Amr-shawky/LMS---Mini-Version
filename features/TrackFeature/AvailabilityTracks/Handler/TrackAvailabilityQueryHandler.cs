using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.AvailabilityTracks.DTOs;
using LMS___Mini_Version.features.TrackFeature.AvailabilityTracks.Queries;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.AvailabilityTracks.Handler;

public class TrackAvailabilityQueryHandler(IGeneralRepository<Track> trackRepository,
    IGeneralRepository<Enrollment> enrollmentRepository)
    : IRequestHandler<AvailabilityQuery, RequestResponse<TrackAvailabilityDto>>
{
    public async Task<RequestResponse<TrackAvailabilityDto>> Handle(
        AvailabilityQuery request, CancellationToken cancellationToken)
    {
        // get track
        var track = await trackRepository.GetByIdAsync(request.TrackId);
        if (track is null)
            return RequestResponse<TrackAvailabilityDto>.Fail(404, new Dictionary<string, string[]>
            {
                { "TrackId", ["Track not found"] }
            });

        // reserved seats (Pending + Active only, counted from Enrollments)
        var reservedSeats = await enrollmentRepository.GetAll()
            .CountReservedSeatsAsync(request.TrackId, cancellationToken);

        // never negative, even if existing data is inconsistent
        var availableSeats = Math.Max(0, track.MaxCapacity - reservedSeats);

        var dto = new TrackAvailabilityDto(
            track.Name,
            track.IsActive,
            track.MaxCapacity,
            reservedSeats,
            availableSeats);

        return RequestResponse<TrackAvailabilityDto>.OK(dto);
    }
}