using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Enrollments.Queries;

public record GetEnrollmentByIdQuery(int Id) : IQuery<EnrollmentDto>;

public class GetEnrollmentByIdHandler (IGeneralRepository<Enrollment> _enrollmentRepo) 
    : IRequestHandler<GetEnrollmentByIdQuery, EnrollmentDto>
{    
    public async Task<EnrollmentDto> Handle(GetEnrollmentByIdQuery request, CancellationToken cancellationToken)
    {

        var enrollment = await _enrollmentRepo
          .GetTable()
          .Where(e => e.Id == request.Id)
          .Select(e => new EnrollmentDto
          {
              Id = e.Id,
              InternName = e.Intern.FullName,
              TrackName = e.Track.Name,
              EnrollmentDate = e.EnrollmentDate,
              Status = e.Status
          }).FirstOrDefaultAsync(cancellationToken);

        return enrollment;
    }
}
