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
        private readonly IGeneralRepository<Enrollment> _generalRepository;
        public GetEnrollmentsByInternQueryHandler(IGeneralRepository<Enrollment> generalRepository)
        {
            _generalRepository = generalRepository;
        }

        public async Task<IEnumerable<EnrollmentDto>> Handle(GetEnrollmentsByInternQuery request, CancellationToken cancellationToken)
        {
            var enrollments = await _generalRepository
    .GetTable()
    .Include(e => e.Track)
    .Include(e => e.Intern)
    .Where(e => e.InternId == request.InternId)
    .ToListAsync(cancellationToken);

            return enrollments.Select(e => e.ToDto());
        }
    }
}
