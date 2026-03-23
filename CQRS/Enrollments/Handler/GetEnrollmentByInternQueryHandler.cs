using LMS___Mini_Version.CQRS.Enrollments.Query;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Infrastructure.DTO_S.EnrollmentDTO_s;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Enrollments.Handler
{
    public class GetEnrollmentByInternQueryHandler(IGeneralRepository<Enrollment> _repository) : IRequestHandler<GetInternByEnrollmentQuery, IEnumerable<InternEnrollmentDto>>
    {
        
        public async Task<IEnumerable<InternEnrollmentDto>> Handle(GetInternByEnrollmentQuery request, CancellationToken cancellationToken)
        {
            var Enrolls =await _repository.GetTable()
                        .Include(e => e.Intern)
                        .Include(e => e.Track)
                        .Where(e => e.InternId == request.internId)
                        .ToListAsync();
            var dto =Enrolls.Select(e=>e.ToEnrollmentInternDto()).ToList();
            return dto;


        }
    }
}
