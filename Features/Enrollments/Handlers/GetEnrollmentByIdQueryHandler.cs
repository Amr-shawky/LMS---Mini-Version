using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Enrollments.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Handlers;

public class GetEnrollmentByIdQueryHandler(IGeneralRepository<Enrollment> _enrollmentrepo) : IRequestHandler<GetEnrollmentByIdQuery, EnrollmentDto?>
{
    public async Task<EnrollmentDto?> Handle(GetEnrollmentByIdQuery request, CancellationToken cancellationToken)
    {
        var enrollment = await _enrollmentrepo.GetTable()
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (enrollment == null)
            return null;

        EnrollmentDto enrollmentDto = new EnrollmentDto
        {
            Id = enrollment.Id,
            InternId = enrollment.InternId,
            TrackId = enrollment.TrackId,
            Status = enrollment.Status,
            EnrollmentDate = enrollment.EnrollmentDate
        };

        return enrollmentDto;
    }
}