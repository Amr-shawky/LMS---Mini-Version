using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Features.Enrollments.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Handlers;

public class GetActiveEnrollmentsCountQueryHandler
    (IGeneralRepository<Enrollment> _enrollmentrepo)
    : IRequestHandler<GetActiveEnrollmentsCountQuery, int>
{

    public async Task<int> Handle(GetActiveEnrollmentsCountQuery request, CancellationToken cancellationToken)
    {
        return await _enrollmentrepo.GetTable()
            .CountAsync(e => e.TrackId == request.TrackId
                             && e.Status != EnrollmentStatus.Cancelled, cancellationToken);
    }
}