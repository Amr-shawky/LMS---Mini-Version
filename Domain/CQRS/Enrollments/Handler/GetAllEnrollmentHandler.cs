using LMS___Mini_Version.Domain.CQRS.Enrollments.Query;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Infrastructure.DTO_S.EnrollmentDTO_s;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Domain.CQRS.Enrollments.Handler
{
    public class GetAllEnrollmentHandler : IRequestHandler<GetAllEnrollmentsQuery, IEnumerable<EnrollmentDTO>>
    {
        private readonly IGeneralRepository<Enrollment> _repository;

        public GetAllEnrollmentHandler(IGeneralRepository<Enrollment> repository)
        {
            _repository=repository;
        }
        public async Task<IEnumerable<EnrollmentDTO>> Handle(GetAllEnrollmentsQuery request, CancellationToken cancellationToken)
        {
            var Enroll =await _repository.GetTable()
                        .Include(e => e.Intern)
                        .Include(e => e.Track)
                        .ToListAsync(cancellationToken)
                        .ConfigureAwait(false); 
            var EnrollDto=Enroll.Select(e=>e.toEnrollmentDTO());
            return EnrollDto;
           
        }
    }
}
