using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.AvailabilityTracks.DTOs;
using LMS___Mini_Version.features.TrackFeature.AvailabilityTracks.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.TrackFeature.AvailabilityTracks.Handler;

public class TrackAvailabilityQueryHandler(
    IGeneralRepository<Track> trackRepository,
    IGeneralRepository<Enrollment> enrollmentRepository)
    : IRequestHandler<AvailabilityQuery, RequestResponse<TrackAvailabilityDto>>
{
    public async Task<RequestResponse<TrackAvailabilityDto>> Handle(
        AvailabilityQuery request, CancellationToken cancellationToken)
    {
        var track = await trackRepository.GetByIdAsync(request.TrackId);
        if (track is null)
            return RequestResponse<TrackAvailabilityDto>.Fail(404, new Dictionary<string, string[]>
            {
                { "TrackId", [$"Track with id {request.TrackId} is not found"] }
            });

        // only Pending and Active enrollments reserve a seat
        var reservedSeats = await enrollmentRepository.GetAll()
            .CountAsync(e => e.TrackId == track.Id &&
                             (e.Status == EnrollmentStatus.Pending ||
                              e.Status == EnrollmentStatus.Active),
                cancellationToken);

        var maxCapacity = track.MaxCapacity;
        var availableSeats = Math.Max(0, maxCapacity - reservedSeats);

        var dto = new TrackAvailabilityDto(
            track.Name, track.IsActive, maxCapacity, reservedSeats, availableSeats);

        return RequestResponse<TrackAvailabilityDto>.OK(dto);
    }
}