using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Queries
{
    public record GetEnrollmentsByInternQuery(int InternId) : IRequest<IEnumerable<EnrollmentDto>>;


    public class GetEnrollmentsByInternQueryHandler : IRequestHandler<GetEnrollmentsByInternQuery, IEnumerable<EnrollmentDto>>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentrepository;

        public GetEnrollmentsByInternQueryHandler(IGeneralRepository<Enrollment> enrollmentrepository)
        {
            _enrollmentrepository = enrollmentrepository;
        }

        // (Include) not a correct way to get a child fields , it get unnecessary fields that i donot use it __ (select *)

        public async Task<IEnumerable<EnrollmentDto>> Handle(GetEnrollmentsByInternQuery request, CancellationToken cancellationToken)
        {
            var enrollments = await _enrollmentrepository
                .GetTable()
                .Include(e => e.Track)
                .Include(e => e.Intern)
                .Where(e => e.InternId == request.InternId)
                .ToListAsync();

            return enrollments.Select(e => e.ToDto());
        }
    }
}
