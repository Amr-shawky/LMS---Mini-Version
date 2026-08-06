using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Enrollments.Queries;

public record GetByInternIdQuery(int internId) : IQuery<IEnumerable<EnrollmentDto>>;

public class GetByInternHandler (IGeneralRepository<Enrollment> _enrollmentRepo) 
    : IRequestHandler<GetByInternIdQuery, IEnumerable<EnrollmentDto>>
{       
    public async Task<IEnumerable<EnrollmentDto>> Handle(GetByInternIdQuery request, CancellationToken cancellationToken)
    {

        var enrollments = await _enrollmentRepo.GetTable()
            .Where(e => e.InternId == request.internId).
            Select(e=> new EnrollmentDto {
                Id = e.Id,
                InternName = e.Intern.FullName,
                TrackName = e.Track.Name,
                EnrollmentDate = e.EnrollmentDate,
                Status = e.Status
            }).ToListAsync();

        return enrollments;
    } 
}
