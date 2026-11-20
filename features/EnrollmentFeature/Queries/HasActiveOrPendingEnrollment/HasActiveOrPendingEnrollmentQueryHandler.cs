using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.Queries.HasActiveOrPendingEnrollment;

public class HasActiveOrPendingEnrollmentQueryHandler(IGeneralRepository<Enrollment> enrollmentRepository)
    : IRequestHandler<HasActiveOrPendingEnrollmentQuery, bool>
{
    public async Task<bool> Handle(HasActiveOrPendingEnrollmentQuery request, CancellationToken ct)
        => await enrollmentRepository.GetAll()
            .AnyAsync(e => e.InternId == request.InternId
                           && e.TrackId == request.TrackId
                           && (e.Status == EnrollmentStatus.Pending
                               || e.Status == EnrollmentStatus.Active),
                ct);
}