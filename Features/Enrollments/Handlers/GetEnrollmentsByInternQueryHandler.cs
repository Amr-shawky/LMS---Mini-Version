using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Enrollments.Queries;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LMS___Mini_Version.Features.Enrollments.Handlers;

public class GetEnrollmentsByInternQueryHandler(IGeneralRepository<Enrollment> enrollmentrepositry) : IRequestHandler<GetEnrollmentsByInternQuery, IEnumerable<EnrollmentDto>>
{
    public async Task<IEnumerable<EnrollmentDto>> Handle(GetEnrollmentsByInternQuery request, CancellationToken cancellationToken)
    {
        var enrollments = await enrollmentrepositry
                          .GetTable()
                          .Where(e => e.InternId == request.Id)
                          .ToListAsync(cancellationToken);

        return enrollments.Select(e=>e.ToDto());


    }
}
