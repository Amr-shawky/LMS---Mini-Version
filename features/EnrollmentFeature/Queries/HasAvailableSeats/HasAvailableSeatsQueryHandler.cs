using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.Queries.HasAvailableSeats;

public class HasAvailableSeatsQueryHandler(IGeneralRepository<Enrollment> enrollmentRepository)
    : IRequestHandler<HasAvailableSeatsQuery, bool>
{
    public async Task<bool> Handle(HasAvailableSeatsQuery request, CancellationToken ct)
    {
        var reservedSeats = await enrollmentRepository.GetAll()
            .CountAsync(e => e.TrackId == request.TrackId
                             && (e.Status == EnrollmentStatus.Pending
                                 || e.Status == EnrollmentStatus.Active),
                ct);

        return reservedSeats < request.MaxCapacity;
    }
}