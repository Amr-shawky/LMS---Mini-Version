using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.shared;

public static class EnrollmentQueryExtensions
{
    // statuses that reserve a seat in a track
    public static readonly EnrollmentStatus[] ReservingStatuses =
        { EnrollmentStatus.Pending, EnrollmentStatus.Active };

    public static Task<int> CountReservedSeatsAsync(
        this IQueryable<Enrollment> enrollments, int trackId, CancellationToken ct)
        => enrollments.CountAsync(
            e => e.TrackId == trackId && ReservingStatuses.Contains(e.Status), ct);
}