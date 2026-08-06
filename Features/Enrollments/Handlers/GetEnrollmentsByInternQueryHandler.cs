using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Enrollments.Queries;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Handlers
{
    public class GetEnrollmentsByInternQueryHandler : IRequestHandler<GetEnrollmentsByInternQuery, IEnumerable<EnrollmentDto>>
    {

        private readonly IGeneralRepository<Enrollment> _enrollment;

        public GetEnrollmentsByInternQueryHandler(IGeneralRepository<Enrollment> enrollment)
        {
            _enrollment = enrollment;
        }


        public async Task<IEnumerable<EnrollmentDto>> Handle(GetEnrollmentsByInternQuery request, CancellationToken cancellationToken)
        {

            var enrollments = await _enrollment
                .GetTableNoTracking()
                .Where(e => e.InternId == request.internId)
                .Select(e => e.ToDto())
               .ToListAsync(cancellationToken);
            
            return enrollments;
        }




    }
}
